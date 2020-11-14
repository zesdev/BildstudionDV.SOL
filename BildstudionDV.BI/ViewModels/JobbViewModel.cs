using BildstudionDV.BI.Models.Jobb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModels
{
    public class JobbViewModel
    {
        public ObjectId Id { get; set; }
        public ObjectId KundId { get; set; }
        public string Title { get; set; }
        public JobbTyp TypAvJobb { get; set; }
        public PrioritetTyp TypAvPrioritet { get; set; }
        public StatusTyp StatusPåJobbet { get; set; }
        public DateTime DatumRegistrerat { get; set; }
        public List<DelJobbViewModel> delJobbs { get; set; }
    }
}