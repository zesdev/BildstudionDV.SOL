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
                return Redirect("../Närvaro/AddDeltagare?message=InkorrektInmatning");
            deltagarLogic.AddDeltagare(viewModel);
            return RedirectToAction("index");

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
        [HttpGet]
        [HttpPost]
        [Authorize]
        public IActionResult Attendence(string date)
        {
            string rawdate = HttpContext.Request.Cookies["userSelectedDate"];
            var dateConverted = Convert.ToDateTime(date);
            if(date != "")
            {
                dateConverted = Convert.ToDateTime(date);
            }
            var compareDate = DateTime.Now.AddYears(-100);
            List<AttendenceViewModel> model = new List<AttendenceViewModel>();
            if(dateConverted > compareDate)
            {
                model = närvaroLogic.GetAttendenceForDate(dateConverted);
                HttpContext.Response.Cookies.Append("userSelectedDate", model.FirstOrDefault().DateConcerning.ToString("yyyy, MM, dd"));
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateNärvaro(AttendenceViewModel viewModel)
        {
            string rawdate = HttpContext.Request.Cookies["userSelectedDate"];
            DateTime date = Convert.ToDateTime(rawdate);
            var deltagare = deltagarLogic.GetAllDeltagare().FirstOrDefault(x => x.DeltagarNamn == viewModel.DeltagarNamn);
            var ogViewModel = närvaroLogic.GetAttendenceForDate(date).First(x => x.DeltagarNamn == viewModel.DeltagarNamn);

            ogViewModel.Måndag = viewModel.Måndag;
            ogViewModel.Tisdag = viewModel.Tisdag;
            ogViewModel.Onsdag = viewModel.Måndag;
            ogViewModel.Torsdag = viewModel.Tisdag;
            ogViewModel.Fredag = viewModel.Måndag;

            närvaroLogic.UpdateAttendence(ogViewModel);
            return RedirectToAction("Attendence", "Närvaro");
        }
    }
}
