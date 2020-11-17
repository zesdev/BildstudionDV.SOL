using MongoDB.Bson;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BildstudionDV.BI.ViewModels
{
    public class GruppViewModel
    {
        public ObjectId Id { get; set; }
        public ObjectId EnhetId { get; set; }
        public string GruppNamn { get; set; }
        public List<InventarieViewModel> InventarierInGrupp { get; set; }
        public int SelectedEnhet { get; set; }
        public List<string> ListOfEnheter { get; set; }
    }
}