using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Index() => View();

        [HttpGet]
        public ViewResult Login() => View();
    }
}
