using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Controllers
{
    public class WikiController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Konton()
        {
            return View();
        }
        [Authorize]
        public IActionResult Inventarier()
        {
            return View();
        }
        [Authorize]
        public IActionResult KundJobb()
        {
            return View();
        }
        [Authorize]
        public IActionResult Närvaro()
        {
            return View();
        }
    }
}
