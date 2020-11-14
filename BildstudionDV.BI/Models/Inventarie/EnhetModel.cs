using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models.Inventarie
{
    public class EnhetModel
    {
        public ObjectId Id { get; set; }
        public string Namn { get; set; }
        public string ChefNamn { get; set; }
    }
}
