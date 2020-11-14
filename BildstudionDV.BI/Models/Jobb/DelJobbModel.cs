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
        public ObjectId Id { get; set; }
        public ObjectId JobbId { get; set; }
        public DelJobbStatus StatusPåJobbet { get; set; }
        public string Namn { get; set; }
        public string VemGör { get; set; }
    }
}
