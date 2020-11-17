using BildstudionDV.BI.ViewModelLogic;
using Microsoft.AspNetCore.Mvc;
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
        public InventarieController(IInventarieVMLogic _inventarieLogic, IGruppVMLogic _gruppLogic, IEnhetVMLogic _enhetLogic)
        {
            inventarieLogic = _inventarieLogic;
            gruppLogic = _gruppLogic;
            enhetLogic = _enhetLogic;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
