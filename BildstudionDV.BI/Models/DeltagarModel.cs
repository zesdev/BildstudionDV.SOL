
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
        public WorkDay Måndag { get; set; }
        public WorkDay Tisdag { get; set; }
        public WorkDay Onsdag { get; set; }
        public WorkDay Torsdag { get; set; }
        public WorkDay Fredag { get; set; }
        public bool IsActive { get; set; }
    }
}