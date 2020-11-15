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
        public IActionResult RemoveDeltagare(int id)
        {
            try
            {
                var deltagaren = deltagarLogic.GetAllDeltagare().First(x => x.IdAcesss == id);
                deltagarLogic.RemoveDeltagare(deltagaren.Id);
            }
            catch
            {
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Index()
        {

            return View(deltagarViewLogic.GetAllDeltagareViewData());
        }
        [Authorize]
        public IActionResult AddDeltagare(string message)
        {
            ViewBag.Message = message;
            ViewBag.Workday = Enum.GetValues(typeof(WorkDay)).Cast<WorkDay>().ToList();
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddDeltagare(DeltagareViewModel viewModel)
        {
            if (viewModel.DeltagarNamn == null)
                return Redirect();
            deltagarLogic.AddDeltagare(viewModel);
            return Redirect("../Närvaro/AddDeltagare?message=InkorrektInmatning");
        }
        [Authorize]
        public IActionResult EditDeltagare(int id, string message)
        {
            try
            {
                var model = deltagarLogic.GetAllDeltagare().First(x => x.IdAcesss == id);
                ViewBag.Workday = Enum.GetValues(typeof(WorkDay)).Cast<WorkDay>().ToList();
                ViewBag.Message = message;
                return View(model);
            }
            catch
            {
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditDeltagare(DeltagareViewModel viewModel)
        {
            var ogDeltagare = deltagarLogic.GetAllDeltagare().First(x => x.IdAcesss == viewModel.IdAcesss);
            viewModel.Id = ogDeltagare.Id;
            if (viewModel.DeltagarNamn == null)
                return Redirect("../Närvaro/EditDeltagare?id="+viewModel.IdAcesss.ToString()+"&message=InkorrektInmatning");
            deltagarLogic.UpdateDeltagare(viewModel);
            return RedirectToAction("index");
        }
    }
}
