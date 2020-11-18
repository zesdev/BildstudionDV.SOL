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
    public class JobbController : Controller
    {
        IJobbVMLogic jobbLogic;
        IKundVMLogic kundLogic;
        IDelJobbVMLogic delJobbLogic;
        public JobbController(IJobbVMLogic _jobbLogic, IDelJobbVMLogic _delJobbLogic, IKundVMLogic _kundLogic)
        {
            jobbLogic = _jobbLogic;
            kundLogic = _kundLogic;
            delJobbLogic = _delJobbLogic;
        }
        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            return View(kundLogic.GetKunder());
        }
        [Authorize]
        public IActionResult AddKund()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddKund(KundViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (model.KundNamn == null)
            {
                ViewBag.ErrorMessage = "Kunden måste ha ett namn!";
                return View();
            }
            kundLogic.AddKund(model);
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult EditKund(string kundNamn)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var model = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == kundNamn);
            model.OldName = model.KundNamn;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditKund(KundViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (model.KundNamn == null)
            {
                ViewBag.ErrorMessage = "Kunden måste ha ett namn!";
                return View(model);
            }
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == model.OldName);
            kund.KundNamn = model.KundNamn;
            kundLogic.UpdateKund(kund);
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Jobb(string KundNamn)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            if (KundNamn == null)
            {
                KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            }
            ViewBag.KundNamn = KundNamn;
            
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var model = jobbLogic.GetJobbsForKund(kund.Id);
            HttpContext.Response.Cookies.Append("userSelectedKund", KundNamn);
            return View(model);
        }

        [Authorize]
        public IActionResult RemoveKund(string kundNamn)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            try
            {
                var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == kundNamn);
                kundLogic.RemoveKund(kund.Id);
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult AddJobb()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            return View(new JobbViewModel { KundNamn = KundNamn });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddJobb(JobbViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == model.KundNamn);
            model.KundId = kund.Id;
            model.DatumRegistrerat = DateTime.Now;
            jobbLogic.AddJobb(model);
            return RedirectToAction("jobb", "jobb", model.KundNamn, null);
        }
        [Authorize]
        public IActionResult EditJobb(int AccessId)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var model = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == AccessId);
            ViewBag.AccessId = AccessId;
            model.KundNamn = KundNamn;
            return View(model);
        }
        [Authorize]
        [Authorize]
        public IActionResult RemoveJobb(int AccessId)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            try
            {
                var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
                var kund = kundLogic.GetKunder().First(x => x.KundNamn == KundNamn);
                var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == AccessId);
                jobbLogic.RemoveJobb(jobb.Id);
            }
            catch
            {

            }
            return RedirectToAction("jobb");
        }
        [HttpPost]
        public IActionResult EditJobb(JobbViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == model.KundNamn);
            var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == model.AccessId);
            jobb.KundId = kund.Id;
            jobb.StatusPåJobbet = model.StatusPåJobbet;
            jobb.Title = model.Title;
            jobb.TypAvJobb = model.TypAvJobb;
            jobb.TypAvPrioritet = model.TypAvPrioritet;
            jobbLogic.UpdateJobb(jobb);
            return RedirectToAction("Jobb");
        }
        [Authorize]
        public IActionResult DelJobb(string AccessId)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            int AccessIdInt;
            if(AccessId == null)
            {
                AccessIdInt = Convert.ToInt32(HttpContext.Request.Cookies["userSelectedJobbAccessId"]);
            }
            else
            {
                AccessIdInt = Convert.ToInt32(AccessId);
            }
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            HttpContext.Response.Cookies.Append("userSelectedJobbAccessId", AccessIdInt.ToString());
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == AccessIdInt);
            var model = delJobbLogic.GetDelJobbsInJobb(jobb.Id);
            ViewBag.JobbTitel = jobb.Title;
            ViewBag.KundNamn = kund.KundNamn;
            ViewBag.AccessId = AccessId;
            return View(model);
        }
        [Authorize]
        public IActionResult RemoveDelJobb(int AccessId)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            try
            {
                var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
                var JobbAccessId = Convert.ToInt32(HttpContext.Request.Cookies["userSelectedJobbAccessId"]);
                var kund = kundLogic.GetKunder().First(x =>x.KundNamn == KundNamn);
                var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == JobbAccessId);
                var deljobb = delJobbLogic.GetDelJobbsInJobb(jobb.Id).FirstOrDefault(x => x.AccessId == AccessId);
                delJobbLogic.RemoveDelJobb(deljobb.Id);
            }
            catch
            {

            }
            return RedirectToAction("DelJobb");
        }
        [Authorize]
        public IActionResult AddDelJobb()
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            var JobbAccessId = Convert.ToInt32(HttpContext.Request.Cookies["userSelectedJobbAccessId"]);
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == JobbAccessId);
            ViewBag.KundNamn = kund.KundNamn;
            ViewBag.JobbTitel = jobb.Title;
            return View(new DelJobbViewModel());
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddDelJobb(DelJobbViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            var JobbAccessId = Convert.ToInt32(HttpContext.Request.Cookies["userSelectedJobbAccessId"]);
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == JobbAccessId);
            model.JobbId = jobb.Id;
            delJobbLogic.AddDelJobb(model);
            return RedirectToAction("DelJobb");
        }
        [Authorize]
        public IActionResult EditDelJobb(int AccessId)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            var JobbAccessId = Convert.ToInt32(HttpContext.Request.Cookies["userSelectedJobbAccessId"]);
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == JobbAccessId);
            var model = delJobbLogic.GetDelJobbsInJobb(jobb.Id).FirstOrDefault(x => x.AccessId == AccessId);
            ViewBag.KundNamn = kund.KundNamn;
            ViewBag.JobbTitel = jobb.Title;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditDelJobb(DelJobbViewModel model)
        {
            if (User.Identity.Name != "admin" && User.Identity.Name != "piahag")
                return RedirectToAction("index", "inventarie");
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            var JobbAccessId = Convert.ToInt32(HttpContext.Request.Cookies["userSelectedJobbAccessId"]);
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var jobb = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == JobbAccessId);
            var deljobb = delJobbLogic.GetDelJobbsInJobb(jobb.Id).FirstOrDefault(x => x.AccessId == model.AccessId);
            deljobb.StatusPåJobbet = model.StatusPåJobbet;
            deljobb.VemGör = model.VemGör;
            deljobb.Namn = model.Namn;
            deljobb.Kommentar = model.Kommentar;
            delJobbLogic.UpdateDelJobb(deljobb);
            return RedirectToAction("DelJobb");
        }
    }
}
