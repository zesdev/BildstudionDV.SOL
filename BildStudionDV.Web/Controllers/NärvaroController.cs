using BildstudionDV.BI.MainLogic;
using BildstudionDV.BI.Models;
using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Controllers
{
    public class NärvaroController : Controller
    {
        INärvaroVMLogic närvaroLogic;
        IDeltagareVMLogic deltagarLogic;
        IDeltagarViewLogic deltagarViewLogic;
        public NärvaroController(INärvaroVMLogic _närvaroLogic, IDeltagareVMLogic _deltagarLogic, IDeltagarViewLogic _deltagarViewLogic)
        {
            närvaroLogic = _närvaroLogic;
            deltagarLogic = _deltagarLogic;
            deltagarViewLogic = _deltagarViewLogic;
        }
        [Authorize]
        public IActionResult Index()
        {

            return View(deltagarViewLogic.GetAllDeltagareViewData());
        }
        [Authorize]
        public IActionResult AddDeltagare()
        {
            ViewBag.Workday = Enum.GetValues(typeof(WorkDay)).Cast<WorkDay>().ToList();
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddDeltagare(DeltagareViewModel viewModel)
        {
            deltagarLogic.AddDeltagare(viewModel);
            return RedirectToAction("index");
            ViewBag.Workday = Enum.GetValues(typeof(WorkDay)).Cast<WorkDay>().ToList();
            return View();
        }
    }
}
