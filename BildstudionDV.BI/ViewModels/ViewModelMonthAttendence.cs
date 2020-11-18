﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModels
{
    public class ViewModelMonthAttendence
    {
        public DeltagareViewModel deltagaren { get; set; }
        public string MånadNamn { get; set; }
        public int PercentageAttendence { get; set; }
        public int AttendendedDays { get; set; }
        public int ExpectedDays { get; set; }
        public int SjukDays { get; set; }
        public int LedigDays { get; set; }
        public int Frånvarande { get; set; }
        public int Halvdagar { get; set; }
        public int Heldagar { get; set; }
    }
}