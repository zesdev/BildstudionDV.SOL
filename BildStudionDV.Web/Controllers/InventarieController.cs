using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
        public InventarieController(IInventarieVMLogic _inventarieLogic, IGruppVMLogic _gruppLogic, IEnhetVMLogic _enhetLogic, IUserProfileVMLogic _userLogic)
        {
            userLogic = _userLogic;
            inventarieLogic = _inventarieLogic;
            gruppLogic = _gruppLogic;
            enhetLogic = _enhetLogic;
        }
        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
            {
                var userModel = userLogic.GetUserViewModels().First(x => x.UserName == User.Identity.Name);
                var grupp = gruppLogic.GetAllGrupper().FirstOrDefault(x => x.GruppNamn.ToLower() == userModel.AssociatedGrupp.ToLower());
                var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Id == grupp.EnhetId);
                return Redirect("../inventarie/grupp?gruppnamn="+userModel.AssociatedGrupp+"&enhetnamn="+enhet.Namn);
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
                var model = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn == namn);
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
                var enhetToEdit = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn == oldEnhetName);
                enhetToEdit.Namn = model.Namn;
                enhetToEdit.ChefNamn = model.ChefNamn;
                enhetLogic.UpdateEnhet(enhetToEdit);
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
            var model = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn == oldEnhetName);
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
                var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn == oldEnhetName);
                model.EnhetId = enhet.Id;
                gruppLogic.AddGrupp(model);
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
            var enheter = enhetLogic.GetAllEnheter();
            var enhet = enheter.FirstOrDefault(x => x.Namn == oldEnhetName);
            var model = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn == namn);
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
                var enheter = enhetLogic.GetAllEnheter();
                var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn == oldEnhetName);
                var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn == oldGruppNamn);
                var selectedenhet = enheter.FirstOrDefault(x => x.Namn == selectEnhet);
                grupp.GruppNamn = model.GruppNamn;
                grupp.EnhetId = selectedenhet.Id;
                HttpContext.Response.Cookies.Append("EnhetSelected", selectedenhet.Namn);
                gruppLogic.UpdateGrupp(grupp);
                return RedirectToAction("enhet");
            }
            ViewBag.error = "Enheten måste ha ett namn";
            return View(model);
        }

        [Authorize]
        public IActionResult Grupp(string gruppnamn, string enhetnamn)
        {
            if (enhetnamn == null)
            {
                if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                    return RedirectToAction("index", "inventarie");
                var gruppName = gruppnamn;
                if (gruppnamn == null)
                {
                    gruppName = HttpContext.Request.Cookies["GruppSelected"];
                }
                var oldEnhetName = HttpContext.Request.Cookies["EnhetSelected"];
                HttpContext.Response.Cookies.Append("GruppSelected", gruppName);

                var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn == oldEnhetName);
                var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn == gruppName);
                var model = inventarieLogic.GetInventarierFörGrupp(grupp.Id);
                return View(model);
            }
            else
            {
                var gruppName = gruppnamn;
                if (gruppnamn == null)
                {
                    gruppName = HttpContext.Request.Cookies["GruppSelected"];
                }
                var oldEnhetName = enhetnamn;
                HttpContext.Response.Cookies.Append("GruppSelected", gruppName);

                var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn.ToLower() == oldEnhetName.ToLower());
                var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
                var model = inventarieLogic.GetInventarierFörGrupp(grupp.Id);
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
                var enhet = enhetLogic.GetAllEnheter().First(x => x.Namn == enhetNamn);
                var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn == name);
                gruppLogic.RemoveGrupp(grupp.Id);
            }
            catch
            {
            }
            return RedirectToAction("Enhet");
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
            var enhetName = HttpContext.Request.Cookies["EnhetSelected"];
            var gruppName = HttpContext.Request.Cookies["GruppSelected"];

            var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn.ToLower() == enhetName.ToLower());
            var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
            model.GruppId = grupp.Id;
            inventarieLogic.AddInventarie(model);
            return RedirectToAction("Grupp");
        }
        [Authorize]
        public IActionResult EditInventarie(string id)
        {
            HttpContext.Response.Cookies.Append("inventarieIndex", id);
            var enhetName = HttpContext.Request.Cookies["EnhetSelected"];
            var gruppName = HttpContext.Request.Cookies["GruppSelected"];
            var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn.ToLower() == enhetName.ToLower());
            var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
            var model = inventarieLogic.GetInventarierFörGrupp(grupp.Id)[Convert.ToInt32(id)];
            model.IndexOfInventarieInList = Convert.ToInt32(id);
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditInventarie(InventarieViewModel model)
        {
            var enhetName = HttpContext.Request.Cookies["EnhetSelected"];
            var gruppName = HttpContext.Request.Cookies["GruppSelected"];
            int inventarieIndex = Convert.ToInt32(HttpContext.Request.Cookies["inventarieIndex"]);
            var enhet = enhetLogic.GetAllEnheter().FirstOrDefault(x => x.Namn.ToLower() == enhetName.ToLower());
            var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
            var inventarie = inventarieLogic.GetInventarierFörGrupp(grupp.Id)[inventarieIndex];
            
            inventarie.Antal = model.Antal;
            inventarie.Fabrikat = model.Fabrikat;
            inventarie.InventarieKommentar = model.InventarieKommentar;
            inventarie.InventarieNamn = model.InventarieNamn;
            inventarie.Pris = model.Pris;

            inventarieLogic.UpdateInventarie(inventarie);

            return RedirectToAction("Grupp");
        }
        [Authorize]
        public IActionResult RemoveInventarie(string index)
        {
            try
            {
                var enhetNamn = HttpContext.Request.Cookies["EnhetSelected"];
                var gruppName = HttpContext.Request.Cookies["GruppSelected"];
                var enhet = enhetLogic.GetAllEnheter().First(x => x.Namn.ToLower() == enhetNamn.ToLower());
                var grupp = gruppLogic.GetGrupperInEnhet(enhet.Id).FirstOrDefault(x => x.GruppNamn.ToLower() == gruppName.ToLower());
                var inventarie = inventarieLogic.GetInventarierFörGrupp(grupp.Id)[Convert.ToInt32(index)];
                inventarieLogic.RemoveInventarie(inventarie.Id);
            }
            catch
            {
            }
            return RedirectToAction("Grupp");
        }
    }
}
