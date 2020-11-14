﻿using BildStudionDV.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Users()
        {
            var uses = new Users();
            return View(uses.GetUsers());
        }
        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
