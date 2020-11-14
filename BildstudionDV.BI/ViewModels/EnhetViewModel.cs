using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModels
{
    public class EnhetViewModel
    {
        public ObjectId Id { get; set; }
        public string Namn { get; set; }
        public string ChefNamn { get; set; }
        public List<GruppViewModel> grupperInEnhet { get; set; }
    }
}
