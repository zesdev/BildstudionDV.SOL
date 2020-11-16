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
            return View(kundLogic.GetKunder());
        }
        [Authorize]
        public IActionResult AddKund()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddKund(KundViewModel model)
        {
            if(model.KundNamn == null)
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
            var model = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == kundNamn);
            model.OldName = model.KundNamn;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditKund(KundViewModel model)
        {
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
            if(KundNamn == null)
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
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            return View(new JobbViewModel { KundNamn = KundNamn });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddJobb(JobbViewModel model)
        {
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == model.KundNamn);
            model.KundId = kund.Id;
            model.DatumRegistrerat = DateTime.Now;
            jobbLogic.AddJobb(model);
            return RedirectToAction("jobb", "jobb", model.KundNamn, null);
        }
        [Authorize]
        public IActionResult EditJobb(int AccessId)
        {
            var KundNamn = HttpContext.Request.Cookies["userSelectedKund"].ToString();
            var kund = kundLogic.GetKunder().FirstOrDefault(x => x.KundNamn == KundNamn);
            var model = jobbLogic.GetJobbsForKund(kund.Id).FirstOrDefault(x => x.AccessId == AccessId);
            model.KundNamn = KundNamn;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditJobb(JobbViewModel model)
        {
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
    }
}
