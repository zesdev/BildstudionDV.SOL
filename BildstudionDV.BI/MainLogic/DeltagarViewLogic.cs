using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BildstudionDV.BI.MainLogic
{
    public class DeltagarViewLogic : IDeltagarViewLogic
    {
        DeltagareVMLogic deltagarLogic;
        NärvaroVMLogic närvaroLogic;
        IDeltagarViewLogic deltagarViewLogic;

        public DeltagarViewLogic(DeltagareVMLogic _deltagarLogic, NärvaroVMLogic _närvaroLogic)
        {
            deltagarLogic = _deltagarLogic;
            närvaroLogic = _närvaroLogic;
        }
        public List<ViewModelDeltagareAttendence> GetAllDeltagareViewData()
        {
            var returningList = new List<ViewModelDeltagareAttendence>();
            var deltagare = deltagarLogic.GetAllDeltagare();
            foreach (var deltagarn in deltagare)
            {
                returningList.Add(GetDeltagareViewData(deltagarn.Id));
            }
            return returningList;
        }
        
        public ViewModelDeltagareAttendence GetDeltagareViewData(ObjectId UserId)
        {
            var deltagarn = deltagarLogic.GetDeltagare(UserId);
            var attendenceData = närvaroLogic.GetAttendenceFörDeltagare(UserId);
            var model = new ViewModelDeltagareAttendence
            {
                Deltagarn = deltagarn,
                AttendenceData = attendenceData
            };
            model = GetAttendence(model, attendenceData, deltagarn);
          
            return model;
        }

        public List<ViewModelMonthAttendence> GetMonthAttendenceForDeltagare(ObjectId UserId, int monthsToLookBack)
        {
            var returningList = new List<ViewModelMonthAttendence>();
            var deltagare = deltagarLogic.GetDeltagare(UserId);
            for (int i = 0; i < monthsToLookBack; i++)
            {
                returningList.Add(GetAttendenceForDeltagareForMonth(monthsToLookBack, DateTime.Now.AddMonths(-i), deltagare));
            }
            return returningList;
        }

        public ViewModelMonthAttendence GetAttendenceForDeltagareForMonth(int monthsToLookBack, DateTime dateTime, DeltagareViewModel deltagarn)
        {
            var model = new ViewModelMonthAttendence
            {
                deltagaren = deltagarn,
                MånadNamn = dateTime.ToString("yyyy-MM")
            };

            var attendenceData = närvaroLogic.GetAttendenceFörDeltagare(deltagarn.Id).Where(x => x.DateConcerning.Month == dateTime.Month && x.DateConcerning.Year == dateTime.Year).ToList();
            var attendenceModel = GetAttendence(new ViewModelDeltagareAttendence(), attendenceData, deltagarn);
            model.AttendendedDays = attendenceModel.AttendendedDays;
            model.ExpectedDays = attendenceModel.ExpectedDays;
            model.Frånvarande = attendenceModel.Frånvarande;
            model.Halvdagar = attendenceModel.Halvdagar;
            model.Heldagar = attendenceModel.Heldagar;
            model.PercentageAttendence = attendenceModel.PercentageAttendence;
            model.SjukDays = attendenceModel.SjukDays;
            return model;
        }

        private ViewModelDeltagareAttendence GetAttendence(ViewModelDeltagareAttendence model, List<AttendenceViewModel> attendenceData, DeltagareViewModel deltagarn)
        {
            int expectedAttendence = 0;
            int actualAttendence = 0;
            int halvdaysAttended = 0;
            int heldaysAttended = 0;
            int sjukDays = 0;
            int ledigDays = 0;
            int frånvarandeDagar = 0;
            foreach (var data in attendenceData)
            {
                if (data.ExpectedMåndag != Models.WorkDay._)
                    expectedAttendence++;
                if (data.ExpectedTisdag != Models.WorkDay._)
                    expectedAttendence++;
                if (data.ExpectedOnsdag != Models.WorkDay._)
                    expectedAttendence++;
                if (data.ExpectedTorsdag != Models.WorkDay._)
                    expectedAttendence++;
                if (data.ExpectedFredag != Models.WorkDay._)
                    expectedAttendence++;

                if (data.ExpectedMåndag == Models.WorkDay.Heldag)
                {
                    if (data.Måndag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Måndag == Models.Attendence.AttendenceOption.HeldagMat)
                    {
                        actualAttendence++;
                        heldaysAttended++;
                    }
                    else if (data.Måndag == Models.Attendence.AttendenceOption.Halvdag ||
                        data.Måndag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        halvdaysAttended++;
                    }
                    else
                    {
                        if (data.Måndag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Måndag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                else if (data.ExpectedMåndag == Models.WorkDay.Halvdag)
                {
                    if (data.Måndag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Måndag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        actualAttendence++;
                        halvdaysAttended++;
                    }
                    else
                    {

                        if (data.Måndag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Måndag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                if (data.ExpectedTisdag == Models.WorkDay.Heldag)
                {
                    if (data.Tisdag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Tisdag == Models.Attendence.AttendenceOption.HeldagMat)
                    {
                        actualAttendence++;
                        heldaysAttended++;
                    }
                    else if (data.Tisdag == Models.Attendence.AttendenceOption.Halvdag ||
                        data.Tisdag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        halvdaysAttended++;
                    }
                    else
                    {
                        if (data.Tisdag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Tisdag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                else if (data.ExpectedTisdag == Models.WorkDay.Halvdag)
                {
                    if (data.Tisdag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Tisdag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        actualAttendence++;
                        halvdaysAttended++;
                    }
                    else
                    {

                        if (data.Tisdag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Tisdag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                if (data.ExpectedOnsdag == Models.WorkDay.Heldag)
                {
                    if (data.Onsdag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Onsdag == Models.Attendence.AttendenceOption.HeldagMat)
                    {
                        actualAttendence++;
                        heldaysAttended++;
                    }
                    else if (data.Onsdag == Models.Attendence.AttendenceOption.Halvdag ||
                        data.Onsdag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        halvdaysAttended++;
                    }
                    else
                    {
                        if (data.Onsdag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Onsdag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                else if (data.ExpectedOnsdag == Models.WorkDay.Halvdag)
                {
                    if (data.Onsdag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Onsdag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        actualAttendence++;
                        halvdaysAttended++;
                    }
                    else
                    {

                        if (data.Onsdag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Onsdag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                if (data.ExpectedTorsdag == Models.WorkDay.Heldag)
                {
                    if (data.Torsdag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Torsdag == Models.Attendence.AttendenceOption.HeldagMat)
                    {
                        actualAttendence++;
                        heldaysAttended++;
                    }
                    else if (data.Torsdag == Models.Attendence.AttendenceOption.Halvdag ||
                        data.Torsdag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        halvdaysAttended++;
                    }
                    else
                    {
                        if (data.Torsdag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Torsdag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                else if (data.ExpectedTorsdag == Models.WorkDay.Halvdag)
                {
                    if (data.Torsdag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Torsdag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        actualAttendence++;
                        halvdaysAttended++;
                    }
                    else
                    {

                        if (data.Torsdag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Torsdag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                            if (data.ExpectedFredag == Models.WorkDay.Heldag)
                {
                    if (data.Fredag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Fredag == Models.Attendence.AttendenceOption.HeldagMat)
                    {
                        actualAttendence++;
                        heldaysAttended++;
                    }
                    else if (data.Fredag == Models.Attendence.AttendenceOption.Halvdag ||
                        data.Fredag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        halvdaysAttended++;
                    }
                    else
                    {
                        if (data.Fredag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Fredag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }
                else if (data.ExpectedFredag == Models.WorkDay.Halvdag)
                {
                    if (data.Fredag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Fredag == Models.Attendence.AttendenceOption.HalvdagMat)
                    {
                        actualAttendence++;
                        halvdaysAttended++;
                    }
                    else
                    {

                        if (data.Fredag == Models.Attendence.AttendenceOption.Sjuk)
                            sjukDays++;
                        else if (data.Fredag == Models.Attendence.AttendenceOption.Ledig)
                            ledigDays++;
                        else
                            frånvarandeDagar++;
                    }
                }

            }
            model.AttendendedDays = actualAttendence;
            model.ExpectedDays = expectedAttendence;
            model.SjukDays = sjukDays;
            model.LedigDays = ledigDays;
            model.Frånvarande = frånvarandeDagar;
            model.Heldagar = heldaysAttended;
            model.Halvdagar = halvdaysAttended;
            actualAttendence = actualAttendence * 100;
            int actualAddedAttendence = 0;
            try
            {
                actualAddedAttendence = (actualAttendence / expectedAttendence);
            }
            catch
            {

            }
            model.PercentageAttendence = actualAddedAttendence;
            return model;
        }

        public List<ViewModelMonthAttendence> GetMonthAttendenceForDeltagare(ObjectId UserId)
        {
            throw new NotImplementedException();
        }

        ViewModelMonthAttendence IDeltagarViewLogic.GetAttendenceForDeltagareForMonth(int monthsToLookBack, DateTime dateTime, DeltagareViewModel deltagarn)
        {
            throw new NotImplementedException();
        }
    }
}
