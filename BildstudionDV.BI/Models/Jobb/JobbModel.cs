using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models.Jobb
{
    public enum JobbTyp
    { 
        Bilder, Administrativt, Film, Packjobb, Övrigt
    };
    public enum PrioritetTyp
    {
        Låg, Medel, Hög
    };
    public enum StatusTyp
    {
        Påbörjat, SkaKollas, VäntarPåKund, KlarOchAvhämtat
    };
    public class JobbModel
    {
        public int AccessId { get; set; }
        public ObjectId Id { get; set; }
        public ObjectId KundId { get; set; }
        public string Title { get; set; }
        public string TypAvJobb { get; set; }
        public string TypAvPrioritet { get; set; }
        public string StatusPåJobbet { get; set; }
        public DateTime DatumRegistrerat { get; set; }
    }
}
