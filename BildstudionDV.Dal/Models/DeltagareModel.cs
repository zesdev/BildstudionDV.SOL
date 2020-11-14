using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.Dal.Models
{
    public class DeltagareModel
    {
        public ObjectId MyProperty { get; set; }
        public string DeltagarNamn { get; set; }
    }
}
