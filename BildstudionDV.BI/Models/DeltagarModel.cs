
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models
{
    public enum WorkDay
    {
        Heldag, Halvdag, _
    };
    public class DeltagareModel
    {
        public ObjectId Id { get; set; }
        public string DeltagarNamn { get; set; }
        public string Måndag { get; set; }
        public string Tisdag { get; set; }
        public string Onsdag { get; set; }
        public string Torsdag { get; set; }
        public string Fredag { get; set; }
        public bool IsActive { get; set; }
    }
}