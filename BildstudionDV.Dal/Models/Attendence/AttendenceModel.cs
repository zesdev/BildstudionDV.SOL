using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.Dal.Models.Attendence
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
        public AttendenceOption NärvaroTyp { get; set; }
    }
}
