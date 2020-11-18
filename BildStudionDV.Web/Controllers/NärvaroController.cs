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
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
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
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            ViewBag.date = DateTime.Now.ToString("yyyy-MM-dd");
            var model = deltagarViewLogic.GetAllDeltagareViewData();
            for (int i = 0; i < model.Count; i++)
            {
                if (i == model.Count - 1)
                {
                    ViewBag.DeltagarNames += "'" + model[i].Deltagarn.DeltagarNamn + "'";
                    ViewBag.DeltagarValue += model[i].PercentageAttendence.ToString();
                    ViewBag.Color += "'rgba(255, 99, 132, 0.2)'";
                }
                else
                {
                    ViewBag.DeltagarNames += "'" + model[i].Deltagarn.DeltagarNamn + "',";
                    ViewBag.DeltagarValue += model[i].PercentageAttendence.ToString() + ",";
                    ViewBag.Color += "'rgba(255, 99, 132, 0.2)',";
                }
            }
            return View(model);
        }
        [Authorize]
        public IActionResult AddDeltagare(string message)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            ViewBag.Message = message;
            ViewBag.Workday = Enum.GetValues(typeof(WorkDay)).Cast<WorkDay>().ToList();
            return View(new DeltagareViewModel { IsActive = true });
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddDeltagare(DeltagareViewModel viewModel)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (viewModel.DeltagarNamn == null)
                return Redirect("../Närvaro/AddDeltagare?message=InkorrektInmatning");
            deltagarLogic.AddDeltagare(viewModel);
            return RedirectToAction("index");

        }
        [Authorize]
        public IActionResult EditDeltagare(int id, string message)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
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
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var ogDeltagare = deltagarLogic.GetAllDeltagare().First(x => x.IdAcesss == viewModel.IdAcesss);
            viewModel.Id = ogDeltagare.Id;
            if (viewModel.DeltagarNamn == null)
                return Redirect("../Närvaro/EditDeltagare?id="+viewModel.IdAcesss.ToString()+"&message=InkorrektInmatning");
            deltagarLogic.UpdateDeltagare(viewModel);
            return RedirectToAction("index");
        }
        [HttpGet]
        [Authorize]
        public IActionResult Attendence(string date)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            string rawdate = HttpContext.Request.Cookies["userSelectedDate"];
            var compareDate = DateTime.Now.AddYears(-100);
            var dateSupplied = Convert.ToDateTime(date);
            List<AttendenceViewModel> model = new List<AttendenceViewModel>();
            if(dateSupplied > compareDate)
            {
                dateSupplied = GetDateAsLastMonday(dateSupplied);
                ViewBag.date = dateSupplied.ToString("yyyy-MM-dd");
                model = närvaroLogic.GetAttendenceForDate(dateSupplied);
                try
                {
                    HttpContext.Response.Cookies.Append("userSelectedDate", model.FirstOrDefault().DateConcerning.ToString("yyyy-MM-dd"));
                }
                catch
                {
                    HttpContext.Response.Cookies.Append("userSelectedDate", dateSupplied.ToString("yyyy-MM-dd"));
                }
            }
            else
            {
                var dateFromCookie = Convert.ToDateTime(rawdate);
                dateFromCookie = GetDateAsLastMonday(dateFromCookie);
                model = närvaroLogic.GetAttendenceForDate(dateFromCookie);
                ViewBag.date = dateFromCookie.ToString("yyyy-MM-dd");
            }
            return View(model);
        }

        private DateTime GetDateAsLastMonday(DateTime dateSupplied)
        {
            if (dateSupplied.DayOfWeek == DayOfWeek.Tuesday)
                return dateSupplied.AddDays(- 1);
            if (dateSupplied.DayOfWeek == DayOfWeek.Wednesday)
                return dateSupplied.AddDays(-2);
            if (dateSupplied.DayOfWeek == DayOfWeek.Thursday)
                return dateSupplied.AddDays(-3);
            if (dateSupplied.DayOfWeek == DayOfWeek.Friday)
                return dateSupplied.AddDays(-4);
            if (dateSupplied.DayOfWeek == DayOfWeek.Saturday)
                return dateSupplied.AddDays(-5);
            if (dateSupplied.DayOfWeek == DayOfWeek.Sunday)
                return dateSupplied.AddDays(-6);
            else
                return dateSupplied;
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateNärvaro(AttendenceViewModel viewModel)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            string rawdate = HttpContext.Request.Cookies["userSelectedDate"];
            DateTime date = Convert.ToDateTime(rawdate);
            var deltagare = deltagarLogic.GetAllDeltagare().FirstOrDefault(x => x.DeltagarNamn == viewModel.DeltagarNamn);
            var ogViewModel = närvaroLogic.GetAttendenceForDate(date).First(x => x.DeltagarNamn == viewModel.DeltagarNamn);

            ogViewModel.Måndag = viewModel.Måndag;
            ogViewModel.Tisdag = viewModel.Tisdag;
            ogViewModel.Onsdag = viewModel.Onsdag;
            ogViewModel.Torsdag = viewModel.Torsdag;
            ogViewModel.Fredag = viewModel.Fredag;

            närvaroLogic.UpdateAttendence(ogViewModel);
            return RedirectToAction("Attendence");
        }
        [Authorize]
        public IActionResult Diagram(int accessid, int month)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if(month == 0)
            {
                month = 12;
            }
            ViewBag.SelectValue = month;
            var deltagare = deltagarLogic.GetAllDeltagare().FirstOrDefault(x => x.IdAcesss == accessid);
            ViewBag.DeltagarNamn = deltagare.DeltagarNamn;
            var model = deltagarViewLogic.GetMonthAttendenceForDeltagare(deltagare.Id, month);
            return View(model);
        }

    }
}
