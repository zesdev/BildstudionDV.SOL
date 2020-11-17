using BildstudionDV.BI.Context;
using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
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
    public class AcountController : Controller
    {
        IUserProfileVMLogic userLogic;
        public AcountController(IUserProfileVMLogic _userLogic)
        {
            userLogic = _userLogic;
        }
        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.Name == "admin" || User.Identity.Name == "piahag")
            {
                return View(userLogic.GetUserViewModels());
            }
            return RedirectToAction("index", "inventarie");
        }
        [Authorize]
        public IActionResult AddUser()
        {
            if (User.Identity.Name == "admin" || User.Identity.Name == "piahag")
            {
                return View(new UserProfileViewModel());
            }
            return RedirectToAction("index", "inventarie");
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddUser(UserProfileViewModel model)
        {
            if (User.Identity.Name == "admin" || User.Identity.Name == "piahag")
            {
                if(model.UserName != "" && model.AssociatedGrupp != "" && model.Password != "")
                {
                    userLogic.CreateUserAccount(model);
                    return RedirectToAction("index");
                }
                ViewBag.error = "Något fel i inmatningen, <br />Användarnamn/lösenord/grupp får ej vara tomt";
                return View(model);
            }
            return RedirectToAction("index", "inventarie");
        }
        [Authorize]
        public IActionResult RemoveUser(string namn)
        {
            if (User.Identity.Name == "admin" || User.Identity.Name == "piahag")
            {
                var user = userLogic.GetUserViewModels().FirstOrDefault(x => x.UserName == namn);
                userLogic.RemoveAccount(user.UserName);
                return RedirectToAction("index");
            }
            return RedirectToAction("index", "inventarie");
        }
        [Authorize]
        public IActionResult EditUser(string namn)
        {
            if (User.Identity.Name == "admin" || User.Identity.Name == "piahag")
            {
                var user = userLogic.GetUserViewModels().FirstOrDefault(x => x.UserName == namn);
                HttpContext.Response.Cookies.Append("userSelected", user.UserName);
                return View(user);
            }
            return RedirectToAction("index", "inventarie");
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditUser(UserProfileViewModel model)
        {
            if (User.Identity.Name == "admin" || User.Identity.Name == "piahag")
            {
                if (model.AssociatedGrupp != null && model.UserName != null)
                {
                    var oldUserName = HttpContext.Request.Cookies["userSelected"];
                    var user = userLogic.GetUserViewModels().FirstOrDefault(x => x.UserName == oldUserName);
                    user.UserName = model.UserName;
                    user.AssociatedGrupp = model.AssociatedGrupp;
                    userLogic.UpdateUser(user);
                    return RedirectToAction("index");
                }
                ViewBag.error = "Något gick fel, användarnamn kan inte vara tomt, likaså med grupptillhörande.";
                return View(model);
            }
            return RedirectToAction("index", "inventarie");
        }
    }
}
