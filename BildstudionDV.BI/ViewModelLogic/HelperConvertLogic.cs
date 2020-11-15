using BildstudionDV.BI.Models;
using BildstudionDV.BI.Models.Attendence;
using BildstudionDV.BI.Models.Jobb;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public static class HelperConvertLogic
    {
        public static StatusTyp GetStatusTypFromString(string text)
        {

            if (text == StatusTyp.SkaKollas.ToString())
                return StatusTyp.SkaKollas;
            else if (text == StatusTyp.VäntarPåKund.ToString())
                return StatusTyp.VäntarPåKund;
            else if (text == StatusTyp.KlarOchAvhämtat.ToString())
                return StatusTyp.KlarOchAvhämtat;
            else
                return StatusTyp.Påbörjat;
        }
        public static PrioritetTyp GetPrioritetTypFromString(string text)
        {
            if (text == PrioritetTyp.Hög.ToString())
                return PrioritetTyp.Hög;
            else if (text == PrioritetTyp.Medel.ToString())
                return PrioritetTyp.Medel;
            else
                return PrioritetTyp.Låg;
        }
        public static JobbTyp GetJobbTypFromString(string text)
        {
            if (text == JobbTyp.Administrativt.ToString())
                return JobbTyp.Administrativt;
            else if (text == JobbTyp.Bilder.ToString())
                return JobbTyp.Bilder;
            else if (text == JobbTyp.Film.ToString())
                return JobbTyp.Film;
            else if (text == JobbTyp.Packjobb.ToString())
                return JobbTyp.Packjobb;
            else
                return JobbTyp.Övrigt;
        }
        public static DelJobbStatus GetDelJobbStatusFromString(string text)
        {
            if (text == DelJobbStatus.Klar.ToString())
                return DelJobbStatus.Klar;
            else
                return DelJobbStatus.AttGöras;
        }
        public static AttendenceOption GetAttendenceOptionFromString(string text)
        {
            if (text == AttendenceOption.Frånvarande.ToString())
                return AttendenceOption.Frånvarande;
            else if (text == AttendenceOption.FrånvarandeMat.ToString())
                return AttendenceOption.FrånvarandeMat;
            else if (text == AttendenceOption.Halvdag.ToString())
                return AttendenceOption.Halvdag;
            else if (text == AttendenceOption.HalvdagMat.ToString())
                return AttendenceOption.HalvdagMat;
            else if (text == AttendenceOption.Heldag.ToString())
                return AttendenceOption.Heldag;
            else if (text == AttendenceOption.HeldagMat.ToString())
                return AttendenceOption.HeldagMat;
            else if (text == AttendenceOption.Ledig.ToString())
                return AttendenceOption.Ledig;
            else if (text == AttendenceOption.Sjuk.ToString())
                return AttendenceOption.Sjuk;
            else 
                return AttendenceOption.Övrigt;
        }
        public static WorkDay GetWorkDayFromString(string text)
        {
            if (text == WorkDay.Heldag.ToString())
                return WorkDay.Heldag;
            else if (text == WorkDay.Halvdag.ToString())
                return WorkDay.Halvdag;
            else return WorkDay._;
        }
    }
}
