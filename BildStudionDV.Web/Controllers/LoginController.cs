using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using BildStudionDV.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace BildStudionDV.Web.Controllers
{
    public class LoginController : Controller
    {
        private IUserProfileVMLogic userLogic;
        public LoginController(IUserProfileVMLogic _userLogic)
        {
            userLogic = _userLogic;
        }
        [HttpGet]
        public ActionResult UserLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult UserLogin([Bind] UserProfileViewModel user)
        {
            var allUsers = userLogic.GetUserViewModels();
            if (allUsers.Any(x => x.UserName.ToLower() == user.UserName))
            {
                if (userLogic.Login(user) == "Inloggad")
                {
                    var userClaims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, ""),
                 };

                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(user);
        }
    }
}
