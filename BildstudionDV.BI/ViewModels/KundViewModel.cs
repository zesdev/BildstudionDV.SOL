using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModels
{
    public class KundViewModel
    {
        public ObjectId Id { get; set; }
        public string KundNamn { get; set; }
        public List<JobbViewModel> listOfJobbs { get; set; }
    }
}
