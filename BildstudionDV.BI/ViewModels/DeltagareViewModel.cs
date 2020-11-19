using BildstudionDV.BI.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;

namespace BildstudionDV.BI.ViewModels
{
    public class DeltagareViewModel
    {
        public ObjectId Id { get; set; }
        public int IdAcesss { get; set; }
        public string DeltagarNamn { get; set; }
        public string MatId { get; set; }
        public WorkDay Måndag { get; set; }
        public WorkDay Tisdag { get; set; }
        public WorkDay Onsdag { get; set; }
        public WorkDay Torsdag { get; set; }
        public WorkDay Fredag { get; set; }
        public bool IsActive { get; set; }
    }
}
