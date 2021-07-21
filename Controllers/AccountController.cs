using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BooksStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IStoreRepository _repository;

        public AccountController(IStoreRepository repository) => _repository = repository;

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login(string returnUrl = null) => View(new UserLogin());

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            userLogin.Password = HashStringWithSHA256(userLogin.Password);
            var user = _repository.FindUser(userLogin.Email, userLogin.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Неправильный логин или пароль!");
                return View();
            }

            await Authenticate(user);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToActionPermanent("Index", "Store");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Register() => View(new UserRegistration());

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistration user)
        {
            if (_repository.CheckEmailAlreadyExists(user.Email))
                ModelState.AddModelError("Email", "Указанный адрес почты уже зарегестрирован!");

            if (ModelState.IsValid == false)
                return View(user);

            user.Role = _repository.FindRole("user");
            user.Password = HashStringWithSHA256(user.Password);
            user.RegistrationTime = DateTime.Now;

            _repository.AddUser(user);

            if (HttpContext != null)
                await Authenticate(user);
            return RedirectToAction("Index", "Store");
        }

        [HttpGet]
        public ViewResult AccessDenied() => View();

        [HttpGet]
        public RedirectToActionResult Logout()
        {
            HttpContext.SignOutAsync("Cookies");
            return RedirectToActionPermanent("Index", "Store");
        }

        [HttpGet]
        public ViewResult Cart() => View(_repository.FindCartItems(_repository.FindUser(User.Identity?.Name).Id));

        [HttpGet]
        public ViewResult Profile()
        {
            var user = _repository.FindUser(HttpContext.User.Identity?.Name);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Profile(User user)
        {
            var userToUpdate = _repository.FindUser(HttpContext.User.Identity?.Name);
            if (user == null)
                return NotFound();

            ModelState.Clear();
            if (user.Email != userToUpdate?.Email && _repository.CheckEmailAlreadyExists(user.Email))
                ModelState.AddModelError("Email", "Указанный адрес почты уже зарегестрирован!");
            user.Password = userToUpdate?.Password;
            if (TryValidateModel(user) == false)
                return View(user);

            if (userToUpdate == null)
                return View(user);
            userToUpdate.Name = user.Name;
            userToUpdate.SecondName = user.SecondName;
            userToUpdate.Email = user.Email;
            userToUpdate.Phone = user.Phone;

            _repository.UpdateUser(userToUpdate);
            TempData["UpdateMessage"] = "Информация была успешно обновлена!";
            if (HttpContext != null)
                await Authenticate(userToUpdate);

            return View(user);
        }

        [HttpGet]
        public IActionResult ChangeTheme(string themeName, string returnUrl = null, string parameters = null)
        {
            switch (themeName)
            {
                case "Light":
                    Response.Cookies.Append("Theme", "Light");
                    break;
                case "Dark":
                    Response.Cookies.Append("Theme", "Dark");
                    break;
                default:
                    return NotFound();
            }

            return string.IsNullOrWhiteSpace(returnUrl) == false
                ? Redirect(returnUrl + parameters)
                : Redirect("/Store/Index");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
        }

        private static string HashStringWithSHA256(string data)
        {
            byte[] hashedBits = SHA256.HashData(Encoding.UTF8.GetBytes(data));
            string hashedData = Regex.Replace(BitConverter.ToString(hashedBits), "-", "");
            return hashedData;
        }
    }
}