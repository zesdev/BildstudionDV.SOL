using BildstudionDV.BI.MainLogic;
using BildstudionDV.BI.Models;
using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Controllers
{
    public class NärvaroController : Controller
    {
        INärvaroVMLogic närvaroLogic;
        IDeltagareVMLogic deltagarLogic;
        IDeltagarViewLogic deltagarViewLogic;
        IMatlistaLogic matlistaLogic;
        public NärvaroController(INärvaroVMLogic _närvaroLogic, IDeltagareVMLogic _deltagarLogic, IDeltagarViewLogic _deltagarViewLogic, IMatlistaLogic _matlistaLogic)
        {
            närvaroLogic = _närvaroLogic;
            deltagarLogic = _deltagarLogic;
            deltagarViewLogic = _deltagarViewLogic;
            matlistaLogic = _matlistaLogic;
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
            
            var compareDate = DateTime.Now.AddYears(-100);
            var dateSupplied = Convert.ToDateTime(date);
            List<AttendenceViewModel> model = new List<AttendenceViewModel>();
            if(dateSupplied > compareDate)
            {
                dateSupplied = GetDateAsLastMonday(dateSupplied);
                ViewBag.date = dateSupplied.ToString("yyyy-MM-dd");
                model = närvaroLogic.GetAttendenceForDate(dateSupplied);
                HttpContext.Response.Cookies.Append("userSelectedDate", dateSupplied.ToString("yyyy-MM-dd"));
                ViewBag.Month = dateSupplied.Month;
                ViewBag.Year = dateSupplied.Year;
            }
            else
            {
                string rawdate = HttpContext.Request.Cookies["userSelectedDate"];
                var dateFromCookie = Convert.ToDateTime(rawdate);
                dateFromCookie = GetDateAsLastMonday(dateFromCookie);
                model = närvaroLogic.GetAttendenceForDate(dateFromCookie);
                ViewBag.date = dateFromCookie.ToString("yyyy-MM-dd");
                ViewBag.Month = dateFromCookie.Month;
                ViewBag.Year = dateFromCookie.Year;
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
            ViewBag.accessid = accessid;
            var deltagare = deltagarLogic.GetAllDeltagare().FirstOrDefault(x => x.IdAcesss == accessid);
            ViewBag.DeltagarNamn = deltagare.DeltagarNamn;
            var model = deltagarViewLogic.GetMonthAttendenceForDeltagare(deltagare.Id, month);
            return View(model);
        }
        [Authorize]
        public IActionResult ExportNärvaro(int accessid, int month)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (month == 0)
                month = 12;
            var deltagare = deltagarLogic.GetAllDeltagare().FirstOrDefault(x => x.IdAcesss == accessid);
            var model = deltagarViewLogic.GetMonthAttendenceForDeltagare(deltagare.Id, month);
            bool alternatingrow = true;
            using (var workbook = new XLWorkbook())
            {
        

                var ws = workbook.Worksheets.Add("Månader");
                var col1 = ws.Column("A");
                col1.Width = 10;
                var col2 = ws.Column("B");
                col2.Width = 9;
                var col3 = ws.Column("C");
                col3.Width = 10;
                var col4 = ws.Column("D");
                col4.Width = 8;
                var col5 = ws.Column("E");
                col5.Width = 9;
                var col6 = ws.Column("F");
                col6.Width = 9;
                var col7 = ws.Column("G");
                col7.Width = 4;
                var col8 = ws.Column("H");
                col8.Width = 4;
                var col9 = ws.Column("I");
                col9.Width = 11;
                workbook.SaveAs("månader.xlsx");
                
                var worksheet = workbook.Worksheet(1);
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Datum";
                worksheet.Cell(currentRow, 2).Value = "Deltagare";
                worksheet.Cell(currentRow, 3).Value = "Närvaro";
                worksheet.Cell(currentRow, 4).Value = "%";
                worksheet.Cell(currentRow, 5).Value = "HelDagar";
                worksheet.Cell(currentRow, 6).Value = "HalvDagar";
                worksheet.Cell(currentRow, 7).Value = "Sjuk";
                worksheet.Cell(currentRow, 8).Value = "Ledig";
                worksheet.Cell(currentRow, 9).Value = "Frånvarande";
                worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 1).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 2).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 3).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 3).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 4).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 4).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 5).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 5).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 6).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 6).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 7).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 7).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 8).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 8).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 9).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 9).Style.Font.FontColor = XLColor.White;
                foreach (var monthModel in model)
                {
                    if (monthModel.ExpectedDays != 0)
                    {
                        if (alternatingrow)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = monthModel.MånadNamn;
                            worksheet.Cell(currentRow, 2).Value = monthModel.deltagaren.DeltagarNamn;
                            worksheet.Cell(currentRow, 3).Value = monthModel.AttendendedDays + "/" + monthModel.ExpectedDays + "dagar";
                            worksheet.Cell(currentRow, 4).Value = monthModel.PercentageAttendence + "%";
                            worksheet.Cell(currentRow, 5).Value = monthModel.Heldagar + "";
                            worksheet.Cell(currentRow, 6).Value = monthModel.Halvdagar + "";
                            worksheet.Cell(currentRow, 7).Value = monthModel.SjukDays + "";
                            worksheet.Cell(currentRow, 8).Value = monthModel.LedigDays + "";
                            worksheet.Cell(currentRow, 9).Value = monthModel.Frånvarande + "";
                            worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 3).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 4).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 6).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 7).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 9).Style.Fill.BackgroundColor = XLColor.LightGray;
                            alternatingrow = false;
                        }
                        else
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = monthModel.MånadNamn;
                            worksheet.Cell(currentRow, 2).Value = monthModel.deltagaren.DeltagarNamn;
                            worksheet.Cell(currentRow, 3).Value = monthModel.AttendendedDays + "/" + monthModel.ExpectedDays + "dagar";
                            worksheet.Cell(currentRow, 4).Value = monthModel.PercentageAttendence + "%";
                            worksheet.Cell(currentRow, 5).Value = monthModel.Heldagar + "";
                            worksheet.Cell(currentRow, 6).Value = monthModel.Halvdagar + "";
                            worksheet.Cell(currentRow, 7).Value = monthModel.SjukDays + "";
                            worksheet.Cell(currentRow, 8).Value = monthModel.LedigDays + "";
                            worksheet.Cell(currentRow, 9).Value = monthModel.Frånvarande + "";
                            alternatingrow = true;
                        }
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "attendence_"+deltagare.DeltagarNamn+"_"+model.FirstOrDefault().MånadNamn+"_till_"+model.LastOrDefault().MånadNamn+".xlsx");
                }
            }
        }
        [Authorize]
        public IActionResult MatLista(int month, int year)
        {
            if (User.Identity.Name != "piahag" && User.Identity.Name != "admin")
                return RedirectToAction("index", "inventarie");
            ViewBag.Month = month;
            ViewBag.Year = year;
            var model = matlistaLogic.GetAttendenceForMonth(month, year);
            return View(model);
        }
        [Authorize]
        public IActionResult ExportMatlista(int month, int year)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (month == 0)
                month = 12;
            var model = matlistaLogic.GetAttendenceForMonth(month, year);
            bool alternatingrow = true;
            using (var workbook = new XLWorkbook())
            {


                var ws = workbook.Worksheets.Add("matlista");
                var col1 = ws.Column("A");
                col1.Width = 10;
                var col2 = ws.Column("B");
                col2.Width = 12;
                var col3 = ws.Column("C");
                col3.Width = 20;
                var col4 = ws.Column("D");
                col4.Width = 15;
                var col5 = ws.Column("E");
                col5.Width = 14;
                var col6 = ws.Column("F");
                col6.Width = 14;
                workbook.SaveAs("matlista.xlsx");

                var worksheet = workbook.Worksheet(1);
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Deltagare";
                worksheet.Cell(currentRow, 2).Value = "Datum";
                worksheet.Cell(currentRow, 3).Value = "MatId";
                worksheet.Cell(currentRow, 4).Value = "Antal";
                worksheet.Cell(currentRow, 5).Value = "Pris(kr)";
                worksheet.Cell(currentRow, 6).Value = "TotalKostnad";
                worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 1).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 2).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 3).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 3).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 4).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 4).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 5).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 5).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(currentRow, 6).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(currentRow, 6).Style.Font.FontColor = XLColor.White;
                foreach (var matlistItem in model)
                {
                    if (matlistItem.Antal != 0)
                    {
                        if (alternatingrow)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = matlistItem.DeltagarNamn;
                            worksheet.Cell(currentRow, 2).Value = matlistItem.DateConcerning.ToString("yyyy-MM");
                            worksheet.Cell(currentRow, 3).Value = matlistItem.MatId.ToString();
                            worksheet.Cell(currentRow, 4).Value = matlistItem.Antal.ToString();
                            worksheet.Cell(currentRow, 5).Value = matlistItem.PrisPerMatlåda.ToString();
                            worksheet.Cell(currentRow, 6).Value = matlistItem.TotalKostnad.ToString();
                            worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 3).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 4).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                            worksheet.Cell(currentRow, 6).Style.Fill.BackgroundColor = XLColor.LightGray;
                            alternatingrow = false;
                        }
                        else
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = matlistItem.DeltagarNamn;
                            worksheet.Cell(currentRow, 2).Value = matlistItem.DateConcerning.ToString("yyyy-MM");
                            worksheet.Cell(currentRow, 3).Value = matlistItem.MatId.ToString();
                            worksheet.Cell(currentRow, 4).Value = matlistItem.Antal.ToString();
                            worksheet.Cell(currentRow, 5).Value = matlistItem.PrisPerMatlåda.ToString();
                            worksheet.Cell(currentRow, 6).Value = matlistItem.TotalKostnad.ToString();
                            alternatingrow = true;
                        }
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Matlista_" + model.FirstOrDefault().DateConcerning.ToString("yyyy-MM")+ ".xlsx");
                }
            }
        }
    }
}
