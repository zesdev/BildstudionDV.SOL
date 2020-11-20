using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Controllers
{
    public class WikiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
