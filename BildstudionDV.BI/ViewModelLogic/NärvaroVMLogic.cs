using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Attendence;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public class NärvaroVMLogic : INärvaroVMLogic
    {
        Närvaro närvaroDb;
        Deltagare deltagareDb;
        public NärvaroVMLogic(Närvaro _närvaroDb, Deltagare _deltagareDb)
        {
            närvaroDb = _närvaroDb;
            deltagareDb = _deltagareDb;
        }
        public void AddNärvaro(AttendenceViewModel viewModel)
        {
            var deltagare = deltagareDb.GetAllDeltagarModels().First(x => x.Id == viewModel.Id);
            var model = new AttendenceModel
            {
                DateConcerning = viewModel.DateConcerning,
                DeltagarIdInQuestion = viewModel.DeltagarIdInQuestion,
                Måndag = viewModel.Måndag.ToString(),
                Tisdag = viewModel.Tisdag.ToString(),
                Onsdag = viewModel.Onsdag.ToString(),
                Torsdag = viewModel.Torsdag.ToString(),
                Fredag = viewModel.Fredag.ToString(),
                ExpectedMåndag = deltagare.Måndag,
                ExpectedTisdag = deltagare.Tisdag,
                ExpectedOnsdag = deltagare.Onsdag,
                ExpectedTorsdag = deltagare.Torsdag,
                ExpectedFredag = deltagare.Fredag
            };
            närvaroDb.AddAttendence(model);
        }
        public List<AttendenceViewModel> GetAttendenceFörDeltagare(ObjectId deltagarId)
        {
            var returningList = new List<AttendenceViewModel>();

            var models = närvaroDb.GetAttendenceItemsFörDeltagare(deltagarId);
            foreach (var model in models)
            {
                var viewModel = new AttendenceViewModel
                {
                    DateConcerning = model.DateConcerning,
                    DeltagarIdInQuestion = model.DeltagarIdInQuestion,
                    Måndag = HelperConvertLogic.GetAttendenceOptionFromString(model.Måndag),
                    Tisdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Tisdag),
                    Onsdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Onsdag),
                    Torsdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Torsdag),
                    Fredag = HelperConvertLogic.GetAttendenceOptionFromString(model.Fredag),
                    ExpectedMåndag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedMåndag),
                    ExpectedTisdag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedTisdag),
                    ExpectedOnsdag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedOnsdag),
                    ExpectedTorsdag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedTorsdag),
                    ExpectedFredag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedFredag),
                    Id = model.Id
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        public List<AttendenceViewModel> GetAllAttendence()
        {
            var returningList = new List<AttendenceViewModel>();

            var models = närvaroDb.GetAllAttendenceItems();
            foreach (var model in models)
            {
                var viewModel = new AttendenceViewModel
                {
                    DateConcerning = model.DateConcerning,
                    DeltagarIdInQuestion = model.DeltagarIdInQuestion,
                    Måndag = HelperConvertLogic.GetAttendenceOptionFromString(model.Måndag),
                    Tisdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Tisdag),
                    Onsdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Onsdag),
                    Torsdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Torsdag),
                    Fredag = HelperConvertLogic.GetAttendenceOptionFromString(model.Fredag),
                    ExpectedMåndag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedMåndag),
                    ExpectedTisdag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedTisdag),
                    ExpectedOnsdag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedOnsdag),
                    ExpectedTorsdag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedTorsdag),
                    ExpectedFredag = HelperConvertLogic.GetWorkDayFromString(model.ExpectedFredag),
                    Id = model.Id
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        public void RemoveAttendenceItem(ObjectId Id)
        {
            närvaroDb.RemoveAttendence(Id);
        }
        public List<AttendenceViewModel> GetAttendenceForDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Tuesday)
                date = date.AddDays(-1);
            else if (date.DayOfWeek == DayOfWeek.Wednesday)
                date = date.AddDays(-2);
            else if (date.DayOfWeek == DayOfWeek.Thursday)
                date = date.AddDays(-3);
            else if (date.DayOfWeek == DayOfWeek.Friday)
                date = date.AddDays(-4);
            else if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(-5);
            else if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(-6);

            var returningList = new List<AttendenceViewModel>();
            var deltagarnaActive = deltagareDb.GetAllDeltagarModels().Where(x => x.IsActive == true).ToList();

            foreach (var deltagare in deltagarnaActive)
            {
                var attendences = närvaroDb.GetAttendenceForDate(date).Where(x => x.DeltagarIdInQuestion == deltagare.Id).ToList();
                if (attendences.Count == 0)
                {
                    var model = new AttendenceModel
                    {
                        DateConcerning = date,
                        DeltagarIdInQuestion = deltagare.Id,
                        ExpectedFredag = deltagare.Fredag,
                        ExpectedTorsdag = deltagare.Torsdag,
                        ExpectedOnsdag = deltagare.Onsdag,
                        ExpectedTisdag = deltagare.Tisdag,
                        ExpectedMåndag = deltagare.Måndag,
                        Fredag = AttendenceOption.Frånvarande.ToString(),
                        Torsdag = AttendenceOption.Frånvarande.ToString(),
                        Onsdag = AttendenceOption.Frånvarande.ToString(),
                        Tisdag = AttendenceOption.Frånvarande.ToString(),
                        Måndag = AttendenceOption.Frånvarande.ToString()
                    };
                    närvaroDb.AddAttendence(model);
                    model = närvaroDb.GetAttendenceItemsFörDeltagare(deltagare.Id).FirstOrDefault(x => x.DateConcerning.DayOfYear == date.DayOfYear && x.DateConcerning.Year == date.Year);
                    var viewModel = new AttendenceViewModel
                    {
                        DateConcerning = date,
                        DeltagarIdInQuestion = deltagare.Id,
                        ExpectedFredag = HelperConvertLogic.GetWorkDayFromString(deltagare.Fredag),
                        ExpectedTorsdag = HelperConvertLogic.GetWorkDayFromString(deltagare.Torsdag),
                        ExpectedOnsdag = HelperConvertLogic.GetWorkDayFromString(deltagare.Onsdag),
                        ExpectedTisdag = HelperConvertLogic.GetWorkDayFromString(deltagare.Tisdag),
                        ExpectedMåndag = HelperConvertLogic.GetWorkDayFromString(deltagare.Måndag),
                        Fredag = AttendenceOption.Frånvarande,
                        Torsdag = AttendenceOption.Frånvarande,
                        Onsdag = AttendenceOption.Frånvarande,
                        Tisdag = AttendenceOption.Frånvarande,
                        Måndag = AttendenceOption.Frånvarande,
                        Id = model.Id
                    };
                    returningList.Add(viewModel);
                }
                else
                {
                    var model = attendences.FirstOrDefault(x => x.DeltagarIdInQuestion == deltagare.Id);
                    var viewModel = new AttendenceViewModel
                    {
                        DateConcerning = date,
                        DeltagarIdInQuestion = deltagare.Id,
                        ExpectedFredag = HelperConvertLogic.GetWorkDayFromString(deltagare.Fredag),
                        ExpectedTorsdag = HelperConvertLogic.GetWorkDayFromString(deltagare.Torsdag),
                        ExpectedOnsdag = HelperConvertLogic.GetWorkDayFromString(deltagare.Onsdag),
                        ExpectedTisdag = HelperConvertLogic.GetWorkDayFromString(deltagare.Tisdag),
                        ExpectedMåndag = HelperConvertLogic.GetWorkDayFromString(deltagare.Måndag),
                        Fredag = HelperConvertLogic.GetAttendenceOptionFromString(model.Fredag),
                        Torsdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Torsdag),
                        Onsdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Onsdag),
                        Tisdag = HelperConvertLogic.GetAttendenceOptionFromString(model.Tisdag),
                        Måndag = HelperConvertLogic.GetAttendenceOptionFromString(model.Måndag)
                    };
                    returningList.Add(viewModel);
                }
            }
            return returningList;
        }
        public void UpdateAttendences(List<AttendenceViewModel> attendences)
        {
            var modelsAttendences = new List<AttendenceModel>();
            foreach (var attendence in attendences)
            {
                var model = new AttendenceModel
                {
                    DateConcerning = attendence.DateConcerning,
                    DeltagarIdInQuestion = attendence.DeltagarIdInQuestion,
                    Måndag = attendence.Måndag.ToString(),
                    Tisdag = attendence.Tisdag.ToString(),
                    Onsdag = attendence.Onsdag.ToString(),
                    Torsdag = attendence.Torsdag.ToString(),
                    Fredag = attendence.Fredag.ToString(),
                    ExpectedMåndag = attendence.ExpectedMåndag.ToString(),
                    ExpectedTisdag = attendence.ExpectedTisdag.ToString(),
                    ExpectedOnsdag = attendence.ExpectedOnsdag.ToString(),
                    ExpectedTorsdag = attendence.ExpectedTorsdag.ToString(),
                    ExpectedFredag = attendence.ExpectedFredag.ToString(),
                    Id = attendence.Id,
                };
                modelsAttendences.Add(model);
            }
            foreach (var model in modelsAttendences)
            {
                närvaroDb.UpdateAttendence(model);
            }
        }
    }
}
