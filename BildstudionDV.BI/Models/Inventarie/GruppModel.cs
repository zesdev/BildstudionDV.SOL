using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models.Inventarie
{
    public class GruppModel
    {
        public ObjectId Id { get; set; }
        public ObjectId EnhetId { get; set; }
        public string GruppNamn { get; set; }
    }
}
