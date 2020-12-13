using BooksStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            User user = _repository.FindUser(userLogin.Email, userLogin.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Неправильный логин или пароль!");
                return View();
            }

            await Authenticate(user);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
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
        public ViewResult Profile()
        {
            User user = _repository.FindUser(HttpContext.User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Profile(User user)
        {
            User userToUpdate = _repository.FindUser(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();

            ModelState.Clear();
            if (user.Email != userToUpdate?.Email && _repository.CheckEmailAlreadyExists(user.Email))
                ModelState.AddModelError("Email", "Указанный адрес почты уже зарегестрирован!");
            user.Password = userToUpdate?.Password;
            if (TryValidateModel(user) == false)
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
            if (themeName == "Light")
                Response.Cookies.Append("Theme", "Light");
            else if (themeName == "Dark")
                Response.Cookies.Append("Theme", "Dark");
            else
                return NotFound();

            if (string.IsNullOrWhiteSpace(returnUrl) == false)
                return Redirect(returnUrl + parameters ?? "");

            return Redirect("/Store/Index");
        }

        private async Task Authenticate(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
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
