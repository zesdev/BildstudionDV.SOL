using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModels
{
    public class GruppViewModel
    {
        public ObjectId Id { get; set; }
        public ObjectId EnhetId { get; set; }
        public string GruppNamn { get; set; }
        public List<InventarieViewModel> InventarierInGrupp { get; set; }
    }
}