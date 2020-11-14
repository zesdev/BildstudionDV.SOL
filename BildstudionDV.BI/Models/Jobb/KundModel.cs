using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models.Jobb
{
    public class KundModel
    {
        public ObjectId Id { get; set; }
        public string KundNamn { get; set; }
    }
}
