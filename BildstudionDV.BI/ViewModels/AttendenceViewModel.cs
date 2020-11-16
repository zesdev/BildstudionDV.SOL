using BildstudionDV.BI.Models;
using BildstudionDV.BI.Models.Attendence;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModels
{
    public class AttendenceViewModel
    {
        public ObjectId Id { get; set; }
        public ObjectId DeltagarIdInQuestion { get; set; }
        public string DeltagarNamn { get; set; }
        public DateTime DateConcerning { get; set; }
        public AttendenceOption Måndag { get; set; }
        public AttendenceOption Tisdag { get; set; }
        public AttendenceOption Onsdag { get; set; }
        public AttendenceOption Torsdag { get; set; }
        public AttendenceOption Fredag { get; set; }
        public WorkDay ExpectedMåndag { get; set; }
        public WorkDay ExpectedTisdag { get; set; }
        public WorkDay ExpectedOnsdag { get; set; }
        public WorkDay ExpectedTorsdag { get; set; }
        public WorkDay ExpectedFredag { get; set; }
    }
}
