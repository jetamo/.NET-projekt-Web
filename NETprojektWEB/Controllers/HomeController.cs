using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NETprojektWEB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NETprojektWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<MainRes> localizer;
        public HomeController(IStringLocalizer<MainRes> _localizer)
        {
            localizer = _localizer; 
        }

        public IActionResult Index()
        {
            ViewBag.psw = localizer.GetString("password");
            ViewBag.login = localizer.GetString("login");
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginForm form)
        {
            if (ModelState.IsValid)
            {
                if(form.Email != "matulda@zvitezi.gg")
                {
                    ModelState.AddModelError("Email", "User not found.");
                }
                if(ModelState.IsValid && form.Password != "spravneheslo")
                {
                    ModelState.AddModelError("Password", "Wrong password");
                }

                if (ModelState.IsValid)
                {
                    this.HttpContext.Session.SetString("username", form.Email);
                    return RedirectToAction("Index", "BookList");
                }
            }

            ViewBag.psw = localizer.GetString("password");
            ViewBag.login = localizer.GetString("login");
            return View(form);
        }

        public IActionResult Logout()
        {
            this.HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
