using BooksStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IStoreRepository _repository;

        public AccountController(IStoreRepository repository) => _repository = repository;

        [HttpGet]
        public ViewResult Login() => View(new UserLogin());

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var hastedBits = SHA256.HashData(Encoding.UTF8.GetBytes(userLogin.Password));
            string hashedPassword = Regex.Replace(BitConverter.ToString(hastedBits), "-", "");
            userLogin.Password = hashedPassword;

            User user = _repository.FindUser(userLogin.Email, userLogin.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Неправильный логин или пароль!");
                return View();
            }

            await Authenticate(user);
            return RedirectToActionPermanent("Index", "Store");
        }

        [HttpGet]
        public ViewResult Registration() => View(new User());

        [HttpPost]
        public IActionResult Registration(User user)
        {
            if (ModelState.IsValid == false)
                return View(user);
            if (_repository.CheckPhoneAlreadyExists(user.Phone))
            {
                ModelState.AddModelError("Phone", "Указанный номер телефона уже зарегестрирован!");
                return View();
            }

            if (_repository.CheckEmailAlreadyExists(user.Email))
            {
                ModelState.AddModelError("Email", "Указанный адрес почты уже зарегестрирован!");
                return View();
            }

            var hashedBits = SHA256.HashData(Encoding.UTF8.GetBytes(user.Password));
            string hashedPassword = Regex.Replace(BitConverter.ToString(hashedBits), "-", "");

            user.Role = _repository.FindRole("user");
            user.Password = hashedPassword;
            user.RegistrationTime = DateTime.Now;

            _repository.AddUser(user);
            return RedirectToAction("Index", "Store");
        }

        private async Task Authenticate(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
        }
    }
}
