using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models.Jobb
{
    public enum DelJobbStatus
    {
        AttGöras, Klar
    };
    public class DelJobbModel
    {
        public int AccessId { get; set; }
        public ObjectId Id { get; set; }
        public ObjectId JobbId { get; set; }
        public string StatusPåJobbet { get; set; }
        public string Namn { get; set; }
        public string VemGör { get; set; }
    }
}
