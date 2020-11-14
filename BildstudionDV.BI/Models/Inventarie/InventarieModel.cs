using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models.Inventarie
{
    public class InventarieModel
    {
        public ObjectId Id { get; set; }
        public string InventarieNamn { get; set; }
        public ObjectId GruppId { get; set; }
        public string InventarieKommentar { get; set; }
        public DateTime DatumRegistrerat { get; set; }
        public string Antal { get; set; }
        public string Fabrikat { get; set; }
        public string Pris { get; set; }
    }
}
