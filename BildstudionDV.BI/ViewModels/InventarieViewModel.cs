using MongoDB.Bson;
using System;

namespace BildstudionDV.BI.ViewModels
{
    public class InventarieViewModel
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