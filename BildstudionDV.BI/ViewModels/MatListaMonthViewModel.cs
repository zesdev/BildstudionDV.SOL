using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModels
{
    public class MatListaMonthViewModel
    {
        public DateTime DateConcerning { get; set; }
        public string DeltagarNamn { get; set; }
        public string MatId { get; set; }
        public int Antal { get; set; }
        public int PrisPerMatlåda { get; set; }
        public int TotalKostnad { get; set; }
    }
}
