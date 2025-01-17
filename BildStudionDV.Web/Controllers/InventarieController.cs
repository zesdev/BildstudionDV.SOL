﻿using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Controllers
{
    public class InventarieController : Controller
    {
        IInventarieVMLogic inventarieLogic;
        IGruppVMLogic gruppLogic;
        IEnhetVMLogic enhetLogic;
        IUserProfileVMLogic userLogic;
        List<UserProfileViewModel> userList;
        List<EnhetViewModel> enheter;
        List<GruppViewModel> grupper;
        public InventarieController(IInventarieVMLogic _inventarieLogic, IGruppVMLogic _gruppLogic, IEnhetVMLogic _enhetLogic, IUserProfileVMLogic _userLogic)
        {
            userLogic = _userLogic;
            inventarieLogic = _inventarieLogic;
            gruppLogic = _gruppLogic;
            enhetLogic = _enhetLogic;
            enheter = enhetLogic.GetAllEnheter();
            userList = userLogic.GetUserViewModels();
            grupper = gruppLogic.GetAllGrupper();
        }
        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
            {
                var userModel = userList.First(x => x.UserName == User.Identity.Name);
                var grupp = grupper.FirstOrDefault(x => x.GruppNamn.ToLower() == userModel.AssociatedGrupp.ToLower());
                var enhet = enheter.FirstOrDefault(x => x.Id == grupp.EnhetId);
                return Redirect("../inventarie/grupp?namn="+userModel.AssociatedGrupp+"&enhetnamn="+enhet.Namn);
            }
            var model = enhetLogic.GetAllEnheter();
            return View(model);
        }
        [Authorize]
        public IActionResult AddEnhet()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            return View(new EnhetViewModel());
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddEnhet(EnhetViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (model.Namn != "")
            {
                enhetLogic.AddEnhet(model);
                enheter = enhetLogic.GetAllEnheter();
                return RedirectToAction("index");
            }
            ViewBag.error = "Enheten måste ha ett namn";
            return View(model);
        }
        [Authorize]
        public IActionResult EditEnhet(string namn)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            try
            {
                var model = enheter.FirstOrDefault(x => x.Namn == namn);
                HttpContext.Response.Cookies.Append("userEnhetSelectedForEditing", model.Namn);
                return View(model);
            }
            catch
            {
                return RedirectToAction("index");
            }

        }
        [HttpPost]
        [Authorize]
        public IActionResult EditEnhet(EnhetViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (model.Namn != "")
            {
                var oldEnhetName = HttpContext.Request.Cookies["userEnhetSelectedForEditing"];
                var enhetToEdit = enheter.FirstOrDefault(x => x.Namn == oldEnhetName);
                enhetToEdit.Namn = model.Namn;
                enhetToEdit.ChefNamn = model.ChefNamn;
                enhetLogic.UpdateEnhet(enhetToEdit);
                enheter = enhetLogic.GetAllEnheter();
                return RedirectToAction("index");
            }
            ViewBag.error = "Enheten måste ha ett namn";
            return View(model);
        }
        [Authorize]
        public IActionResult Enhet(string namn)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var oldEnhetName = namn;
            if(namn == null)
            {
                oldEnhetName = HttpContext.Request.Cookies["EnhetSelected"];
            }
            HttpContext.Response.Cookies.Append("EnhetSelected", oldEnhetName);
            var model = enheter.FirstOrDefault(x => x.Namn == oldEnhetName);
            return View(model);
        }
        [Authorize]
        public IActionResult AddGrupp()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            return View(new GruppViewModel());
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddGrupp(GruppViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (model.GruppNamn != "")
            {
                var oldEnhetName = HttpContext.Request.Cookies["EnhetSelected"];
                var enhet = enheter.FirstOrDefault(x => x.Namn == oldEnhetName);
                model.EnhetId = enhet.Id;
                gruppLogic.AddGrupp(model);
                enheter = enhetLogic.GetAllEnheter();
                grupper = gruppLogic.GetAllGrupper();
                return RedirectToAction("enhet");
            }
            ViewBag.error = "Enheten måste ha ett namn";
            return View(model);
        }
        [Authorize]
        public IActionResult EditGrupp(string namn)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var oldEnhetName = HttpContext.Request.Cookies["EnhetSelected"];
            var enhet = enheter.FirstOrDefault(x => x.Namn == oldEnhetName);
            var model = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn == namn);
            model.SelectedEnhet = enheter.IndexOf(enhet);
            var list = new List<string>();
            foreach (var enhetNamn in enheter)
            {
                list.Add(enhetNamn.Namn);
            }
            model.ListOfEnheter = list;
            HttpContext.Response.Cookies.Append("OldGruppNamn", namn);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditGrupp(GruppViewModel model, string selectEnhet)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (model.GruppNamn != "")
            {
                var oldEnhetName = HttpContext.Request.Cookies["EnhetSelected"];
                var oldGruppNamn = HttpContext.Request.Cookies["OldGruppNamn"];
                var enhet = enheter.FirstOrDefault(x => x.Namn == oldEnhetName);
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn == oldGruppNamn);
                var selectedenhet = enheter.FirstOrDefault(x => x.Namn == selectEnhet);
                grupp.GruppNamn = model.GruppNamn;
                grupp.EnhetId = selectedenhet.Id;
                HttpContext.Response.Cookies.Append("EnhetSelected", selectedenhet.Namn);
                gruppLogic.UpdateGrupp(grupp);
               
                grupper = gruppLogic.GetAllGrupper();
                enheter = enhetLogic.GetAllEnheter();
                
                return RedirectToAction("enhet");
            }
            ViewBag.error = "Enheten måste ha ett namn";
            return View(model);
        }

        [Authorize]
        public IActionResult Grupp(string namn, string enhetnamn)
        {
            if (enhetnamn == null)
            {
                if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                    return RedirectToAction("index", "inventarie");
                var gruppName = namn;
                if (namn == null)
                {
                    gruppName = HttpContext.Request.Cookies["GruppSelected"];
                }
                var oldEnhetName = HttpContext.Request.Cookies["EnhetSelected"];
                HttpContext.Response.Cookies.Append("GruppSelected", gruppName);

                var enhet = enheter.FirstOrDefault(x => x.Namn == oldEnhetName);
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn == gruppName);
                var model = grupp.InventarierInGrupp;
                ViewBag.GruppNamn = grupp.GruppNamn;
                ViewBag.EnhetNamn = enhet.Namn;
                return View(model);
            }
            else
            {
                var gruppName = namn;
                if (namn == null)
                {
                    gruppName = HttpContext.Request.Cookies["GruppSelected"];
                }
                var oldEnhetName = enhetnamn;
                if(enhetnamn == null)
                {
                    oldEnhetName = HttpContext.Request.Cookies["EnhetSelected"];
                }
                HttpContext.Response.Cookies.Append("GruppSelected", gruppName);
                HttpContext.Response.Cookies.Append("EnhetSelected", oldEnhetName);


                var enhet = enheter.FirstOrDefault(x => x.Namn.ToLower() == oldEnhetName.ToLower());
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
                var model = grupp.InventarierInGrupp;
                ViewBag.GruppNamn = grupp.GruppNamn;
                ViewBag.EnhetNamn = enhet.Namn;
                return View(model);
            }
        }
        [Authorize]
        public IActionResult RemoveGrupp(string name)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            try
            {
                var enhetNamn = HttpContext.Request.Cookies["EnhetSelected"];
                var enhet = enheter.First(x => x.Namn == enhetNamn);
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn == name);
                gruppLogic.RemoveGrupp(grupp.Id);
                enheter = enhetLogic.GetAllEnheter();
                grupper = gruppLogic.GetAllGrupper();
            }
            catch
            {
            }
            return RedirectToAction("Enhet");
        }
        [Authorize]
        public IActionResult RemoveEnhet(string name)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            try
            {
                var enhet = enheter.First(x => x.Namn.ToLower() == name.ToLower());
                
                enhetLogic.RemoveEnhet(enhet.Id);
                enheter = enhetLogic.GetAllEnheter();
                grupper = gruppLogic.GetAllGrupper();
            }
            catch
            {
            }
            return RedirectToAction("index");
        }
        [Authorize]
        public IActionResult AddInventarie()
        {
            ViewBag.GruppNamn = HttpContext.Request.Cookies["GruppSelected"];
            return View(new InventarieViewModel());
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddInventarie(InventarieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var enhetName = HttpContext.Request.Cookies["EnhetSelected"];
                var gruppName = HttpContext.Request.Cookies["GruppSelected"];
              
                var enhet = enheter.FirstOrDefault(x => x.Namn.ToLower() == enhetName.ToLower());
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
                model.GruppId = grupp.Id;
                inventarieLogic.AddInventarie(model);

                enheter = enhetLogic.GetAllEnheter();
                grupper = gruppLogic.GetAllGrupper();
                
                return RedirectToAction("Grupp");
            }
            ViewBag.ErrorMessage = "Nåt fält fattas, se över fälten allt ska vara ifyllt";
            return View(model);
        }
        [Authorize]
        public IActionResult EditInventarie(string id)
        {
            HttpContext.Response.Cookies.Append("inventarieIndex", id);
            var enhetName = HttpContext.Request.Cookies["EnhetSelected"];
            var gruppName = HttpContext.Request.Cookies["GruppSelected"];
            var enhet = enheter.FirstOrDefault(x => x.Namn.ToLower() == enhetName.ToLower());
            var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
            var model = grupp.InventarierInGrupp[Convert.ToInt32(id)];
            model.IndexOfInventarieInList = Convert.ToInt32(id);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditInventarie(InventarieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var enhetName = HttpContext.Request.Cookies["EnhetSelected"];
                var gruppName = HttpContext.Request.Cookies["GruppSelected"];
                int inventarieIndex = Convert.ToInt32(HttpContext.Request.Cookies["inventarieIndex"]);
                var enhet = enheter.FirstOrDefault(x => x.Namn.ToLower() == enhetName.ToLower());
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
                var inventarie = grupp.InventarierInGrupp[inventarieIndex];

                inventarie.Antal = model.Antal;
                inventarie.Fabrikat = model.Fabrikat;
                inventarie.Kommentar = model.Kommentar;
                inventarie.Namn = model.Namn;
                inventarie.Pris = model.Pris;

                inventarieLogic.UpdateInventarie(inventarie);

                enheter = enhetLogic.GetAllEnheter();
                grupper = gruppLogic.GetAllGrupper();
                return RedirectToAction("Grupp");
            }
            ViewBag.ErrorMessage = "Nåt fält fattas, se över fälten allt ska vara ifyllt";
            return View(model);
        }
        [Authorize]
        public IActionResult RemoveInventarie(string index)
        {
            try
            {
                var enhetNamn = HttpContext.Request.Cookies["EnhetSelected"];
                var gruppName = HttpContext.Request.Cookies["GruppSelected"];
                var enhet = enheter.First(x => x.Namn.ToLower() == enhetNamn.ToLower());
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
                var inventarie = grupp.InventarierInGrupp[Convert.ToInt32(index)];
                inventarieLogic.RemoveInventarie(inventarie.Id);

                enheter = enhetLogic.GetAllEnheter();
                grupper = gruppLogic.GetAllGrupper();
            }
            catch
            {
            }
            return RedirectToAction("Grupp");
        }
        [Authorize]
        public IActionResult ExportInventarie(string gruppnamn, string enhetnamn)
        {
            try
            {
                var enhet = enheter.FirstOrDefault(x => x.Namn == enhetnamn);
                var grupp = enhet.grupperInEnhet.FirstOrDefault(x => x.GruppNamn == gruppnamn);
                var model = grupp.InventarierInGrupp;
                bool alternatingrow = true;
                using (var workbook = new XLWorkbook())
                {


                    var ws = workbook.Worksheets.Add("inventarie");
                    var col1 = ws.Column("A");
                    col1.Width = 10;
                    var col2 = ws.Column("B");
                    col2.Width = 18;
                    var col3 = ws.Column("C");
                    col3.Width = 5;
                    var col4 = ws.Column("D");
                    col4.Width = 15;
                    var col5 = ws.Column("E");
                    col5.Width = 14;
                    var col6 = ws.Column("F");
                    col6.Width = 50;
                    workbook.SaveAs("inventarie.xlsx");

                    var worksheet = workbook.Worksheet(1);
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Datum";
                    worksheet.Cell(currentRow, 2).Value = "InventarieTitel";
                    worksheet.Cell(currentRow, 3).Value = "Antal";
                    worksheet.Cell(currentRow, 4).Value = "Fabrikat";
                    worksheet.Cell(currentRow, 5).Value = "Pris";
                    worksheet.Cell(currentRow, 6).Value = "Kommentar";
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
                    foreach (var inventarie in model)
                    {
                            if (alternatingrow)
                            {
                                currentRow++;
                                worksheet.Cell(currentRow, 1).Value = inventarie.DatumRegistrerat.ToString("yyyy-MM-dd");
                                worksheet.Cell(currentRow, 2).Value = inventarie.Namn;
                                worksheet.Cell(currentRow, 3).Value = inventarie.Antal;
                                worksheet.Cell(currentRow, 4).Value = inventarie.Fabrikat;
                                worksheet.Cell(currentRow, 5).Value = inventarie.Pris;
                                worksheet.Cell(currentRow, 6).Value = inventarie.Kommentar;
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
                            worksheet.Cell(currentRow, 1).Value = inventarie.DatumRegistrerat.ToString("yyyy-MM-dd");
                            worksheet.Cell(currentRow, 2).Value = inventarie.Namn;
                            worksheet.Cell(currentRow, 3).Value = inventarie.Antal;
                            worksheet.Cell(currentRow, 4).Value = inventarie.Fabrikat;
                            worksheet.Cell(currentRow, 5).Value = inventarie.Pris;
                            worksheet.Cell(currentRow, 6).Value = inventarie.Kommentar;
                            alternatingrow = true;
                            }
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "inventarier_" + grupp.GruppNamn + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");
                    }

                }
            }
            catch
            {
                return RedirectToAction("index");
            }
        }
        [Authorize]
        public IActionResult ExportAllGrupperInventarie()
        {
            if(!Directory.Exists("inventarieListor"))
                System.IO.Directory.CreateDirectory("inventarieListor");
            
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var fileNameList = new List<string>();
            foreach (var enhet in enheter)
            {
                foreach (var grupp in enhet.grupperInEnhet)
                {
                    try
                    {

                        var model = inventarieLogic.GetInventarierFörGrupp(grupp.Id);
                        bool alternatingrow = true;
                        using (var workbook = new XLWorkbook())
                        {


                            var ws = workbook.Worksheets.Add("inventarie");
                            var col1 = ws.Column("A");
                            col1.Width = 10;
                            var col2 = ws.Column("B");
                            col2.Width = 18;
                            var col3 = ws.Column("C");
                            col3.Width = 5;
                            var col4 = ws.Column("D");
                            col4.Width = 15;
                            var col5 = ws.Column("E");
                            col5.Width = 14;
                            var col6 = ws.Column("F");
                            col6.Width = 50;
                            workbook.SaveAs("inventarie.xlsx");

                            var worksheet = workbook.Worksheet(1);
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Datum";
                            worksheet.Cell(currentRow, 2).Value = "InventarieTitel";
                            worksheet.Cell(currentRow, 3).Value = "Antal";
                            worksheet.Cell(currentRow, 4).Value = "Fabrikat";
                            worksheet.Cell(currentRow, 5).Value = "Pris";
                            worksheet.Cell(currentRow, 6).Value = "Kommentar";
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
                            foreach (var inventarie in model)
                            {
                                if (alternatingrow)
                                {
                                    currentRow++;
                                    worksheet.Cell(currentRow, 1).Value = inventarie.DatumRegistrerat.ToString("yyyy-MM-dd");
                                    worksheet.Cell(currentRow, 2).Value = inventarie.Namn;
                                    worksheet.Cell(currentRow, 3).Value = inventarie.Antal;
                                    worksheet.Cell(currentRow, 4).Value = inventarie.Fabrikat;
                                    worksheet.Cell(currentRow, 5).Value = inventarie.Pris;
                                    worksheet.Cell(currentRow, 6).Value = inventarie.Kommentar;
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
                                    worksheet.Cell(currentRow, 1).Value = inventarie.DatumRegistrerat.ToString("yyyy-MM-dd");
                                    worksheet.Cell(currentRow, 2).Value = inventarie.Namn;
                                    worksheet.Cell(currentRow, 3).Value = inventarie.Antal;
                                    worksheet.Cell(currentRow, 4).Value = inventarie.Fabrikat;
                                    worksheet.Cell(currentRow, 5).Value = inventarie.Pris;
                                    worksheet.Cell(currentRow, 6).Value = inventarie.Kommentar;
                                    alternatingrow = true;
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                var fileName = "inventarieListor/"+grupp.GruppNamn + "_inventarier.xlsx";
                                fileNameList.Add(fileName);
                                workbook.SaveAs(fileName);
                                var content = stream.ToArray();
                            }
                        };
                   
                    }
                    catch
                    {
                    }
                }
            }
            string startPath = "inventarieListor";
            string zipPath = "inventarier.zip";
            if (System.IO.File.Exists("inventarier.zip"))
                System.IO.File.Delete("inventarier.zip");
            ZipFile.CreateFromDirectory(startPath, zipPath);
            var bytes = System.IO.File.ReadAllBytes("inventarier.zip");
            return File(bytes,"application/zip", "inventarier.zip");
        }

    }
}
