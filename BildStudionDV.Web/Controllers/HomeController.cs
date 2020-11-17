using BildstudionDV.BI.Context;
using BildstudionDV.BI.ViewModelLogic;
using BildStudionDV.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserProfileVMLogic userLogic;

        public HomeController(IUserProfileVMLogic _userLogic)
        {
            userLogic = _userLogic;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
