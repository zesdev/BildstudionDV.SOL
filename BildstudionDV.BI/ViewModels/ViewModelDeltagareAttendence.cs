using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModels
{
    public class ViewModelDeltagareAttendence
    {
        public DeltagareViewModel Deltagarn { get; set; }
        public int PercentageAttendence { get; set; }
        public List<AttendenceViewModel> AttendenceData { get; set; }
    }
}
