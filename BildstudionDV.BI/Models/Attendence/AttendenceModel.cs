using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models.Attendence
{
    public enum AttendenceOption
    {
        Heldag, Halvdag, Sjuk, Ledig, Frånvarande, HeldagMat, HalvdagMat, FrånvarandeMat, Övrigt
    };
    public class AttendenceModel
    {
        public ObjectId Id { get; set; }
        public ObjectId DeltagarIdInQuestion { get; set; }
        public DateTime DateConcerning { get; set; }
        public string Måndag { get; set; }
        public string Tisdag { get; set; }
        public string Onsdag { get; set; }
        public string Torsdag { get; set; }
        public string Fredag { get; set; }
        public string ExpectedMåndag { get; set; }
        public string ExpectedTisdag { get; set; }
        public string ExpectedOnsdag { get; set; }
        public string ExpectedTorsdag { get; set; }
        public string ExpectedFredag { get; set; }
    }
}
