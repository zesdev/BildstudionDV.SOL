using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
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
            int percentAttendence = GetAttendence(deltagarn, attendenceData);
            var model = new ViewModelDeltagareAttendence
            {
                Deltagarn = deltagarn,
                AttendenceData = attendenceData,
                PercentageAttendence = percentAttendence
            };
            return model;
        }

        private int GetAttendence(DeltagareViewModel deltagarn, List<AttendenceViewModel> attendenceData)
        {
            int expectedAttendence = 0;
            int actualAttendence = 0;
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
                        actualAttendence++;
                }
                else if (data.ExpectedMåndag == Models.WorkDay.Halvdag)
                {
                    if (data.Måndag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Måndag == Models.Attendence.AttendenceOption.Halvdag)
                        actualAttendence++;
                }
                if (data.ExpectedTisdag == Models.WorkDay.Heldag)
                {
                    if (data.Tisdag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Tisdag == Models.Attendence.AttendenceOption.HeldagMat)
                        actualAttendence++;
                }
                else if (data.ExpectedTisdag == Models.WorkDay.Halvdag)
                {
                    if (data.Tisdag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Tisdag == Models.Attendence.AttendenceOption.Halvdag)
                        actualAttendence++;
                }
                if (data.ExpectedOnsdag == Models.WorkDay.Heldag)
                {
                    if (data.Onsdag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Onsdag == Models.Attendence.AttendenceOption.HeldagMat)
                        actualAttendence++;
                }
                else if (data.ExpectedOnsdag == Models.WorkDay.Halvdag)
                {
                    if (data.Onsdag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Onsdag == Models.Attendence.AttendenceOption.Halvdag)
                        actualAttendence++;
                }
                if (data.ExpectedTorsdag == Models.WorkDay.Heldag)
                {
                    if (data.Torsdag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Torsdag == Models.Attendence.AttendenceOption.HeldagMat)
                        actualAttendence++;
                }
                else if (data.ExpectedTorsdag == Models.WorkDay.Halvdag)
                {
                    if (data.Torsdag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Torsdag == Models.Attendence.AttendenceOption.Halvdag)
                        actualAttendence++;
                }
                if (data.ExpectedFredag == Models.WorkDay.Heldag)
                {
                    if (data.Fredag == Models.Attendence.AttendenceOption.Heldag ||
                   data.Fredag == Models.Attendence.AttendenceOption.HeldagMat)
                        actualAttendence++;
                }
                else if (data.ExpectedTisdag == Models.WorkDay.Halvdag)
                {
                    if (data.Fredag == Models.Attendence.AttendenceOption.Halvdag ||
               data.Fredag == Models.Attendence.AttendenceOption.Halvdag)
                        actualAttendence++;
                }

            }
            expectedAttendence = expectedAttendence * 100;
            actualAttendence = actualAttendence * 100;
            int actualAddedAttendence = 0;
            try
            {
                actualAddedAttendence = actualAttendence / expectedAttendence;
            }
            catch
            {

            }

            return actualAttendence;
        }
    }
}
