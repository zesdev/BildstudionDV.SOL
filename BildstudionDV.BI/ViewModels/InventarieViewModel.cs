using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;

namespace BildstudionDV.BI.ViewModels
{
    public class InventarieViewModel
    {
        public ObjectId Id { get; set; }
        [Required]
        public string Namn { get; set; }
        public ObjectId GruppId { get; set; }
        public string Kommentar { get; set; }
        public DateTime DatumRegistrerat { get; set; }
        [Required]
        public string Antal { get; set; }
        [Required]
        public string Fabrikat { get; set; }
        [Required]
        public string Pris { get; set; }
        public int IndexOfInventarieInList { get; set; }
    }
}