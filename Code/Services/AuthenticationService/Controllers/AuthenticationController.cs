using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void LogInGoogle()
        {

        }

        [HttpPost]
        public void LogIn()
        {

        }

        [HttpGet]
        public void LogOut(string mailId)
        {

        }

        [HttpPost]
        public void Register()
        {

        }

        [HttpPost]
        public void RegisterGoogle()
        {

        }
    }
}
