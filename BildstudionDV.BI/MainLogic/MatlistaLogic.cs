using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models;
using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BildstudionDV.BI.MainLogic
{
    public class MatlistaLogic : IMatlistaLogic
    {
        IMongoCollection<MatLådePris> PrisDb;
        BildStudionDVContext context;
        NärvaroVMLogic närvaroLogic;
        DeltagareVMLogic deltagarLogic;

        public MatlistaLogic(BildStudionDVContext _context, NärvaroVMLogic _närvaroLogic, DeltagareVMLogic _deltagarLogic)
        {
            context = _context;
            närvaroLogic = _närvaroLogic;
            deltagarLogic = _deltagarLogic;
            PrisDb = context.database.GetCollection<MatLådePris>("matlådapriser");
            var list = PrisDb.Find(x => true).ToList();
            if (list.Count == 0)
            {
                PrisDb.InsertOne(new MatLådePris { Pris = 55 });
            }
        }
        public void UpdatePris(int newPris)
        {
            if (newPris != 0)
            {
                var oldone = PrisDb.Find(x => true).FirstOrDefault();
                PrisDb.FindOneAndDelete<MatLådePris>(x => x.Id == oldone.Id);
                PrisDb.InsertOne(new MatLådePris { Pris = newPris });

            }
        }
        public List<MatListaMonthViewModel> GetAttendenceForMonth(int month, int year)
        {
            var returningList = new List<MatListaMonthViewModel>();

            var attendenceData = närvaroLogic.GetAllAttendence().Where(x => x.DateConcerning.Month == month && x.DateConcerning.Year == year);
            var deltagarna = deltagarLogic.GetAllDeltagare();
            foreach (var deltagare in deltagarna)
            {

                int beställningarAntal = 0;
                var deltagarAttendence = attendenceData.Where(x => x.DeltagarIdInQuestion == deltagare.Id).OrderBy(x => x.DateConcerning).ToList();
                if(deltagarAttendence.Count != 0)
                { 
                var firstWeekDate = deltagarAttendence.FirstOrDefault().DateConcerning;
                var check3DaysBeforeIfSameMonth = firstWeekDate.AddDays(-2).Month;
                    if (check3DaysBeforeIfSameMonth == firstWeekDate.AddDays(1).Month)
                    {
                        // kolla sista datumet på sista veckan månaden innan
                        var daysToCountFromFridayBackwars = 0;
                        if (firstWeekDate.AddDays(-5).Month == firstWeekDate.Month)
                            daysToCountFromFridayBackwars = 3;
                        else if (firstWeekDate.AddDays(-4).Month == firstWeekDate.Month)
                            daysToCountFromFridayBackwars = 2;
                        else if (firstWeekDate.AddDays(-3).Month == firstWeekDate.Month)
                            daysToCountFromFridayBackwars = 1;
                        var oldmonth = month - 1;
                        var oldyear = year;
                        if (month == 1)
                        {
                            oldmonth = 12;
                            oldyear = year - 1;
                        }
                        var attendenceForDatWeek = närvaroLogic.GetAllAttendence().Where(x => x.DateConcerning.Month == oldmonth && x.DateConcerning.Year == oldyear && x.DeltagarIdInQuestion == deltagare.Id).OrderByDescending(x => x.DateConcerning).ToList().FirstOrDefault();
                        if (attendenceForDatWeek != null)
                        {
                            if (attendenceForDatWeek.Fredag == Models.Attendence.AttendenceOption.HeldagMat ||
                                attendenceForDatWeek.Fredag == Models.Attendence.AttendenceOption.HalvdagMat ||
                                attendenceForDatWeek.Fredag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                beställningarAntal++;
                            if (daysToCountFromFridayBackwars >= 1)
                            {
                                if (attendenceForDatWeek.Torsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                  attendenceForDatWeek.Torsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                  attendenceForDatWeek.Torsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                    beställningarAntal++;
                            }
                            if (daysToCountFromFridayBackwars >= 2)
                            {
                                if (attendenceForDatWeek.Onsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                  attendenceForDatWeek.Onsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                  attendenceForDatWeek.Onsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                    beställningarAntal++;
                            }
                            if (daysToCountFromFridayBackwars == 3)
                            {
                                if (attendenceForDatWeek.Tisdag == Models.Attendence.AttendenceOption.HeldagMat ||
                  attendenceForDatWeek.Tisdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                  attendenceForDatWeek.Tisdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                    beställningarAntal++;
                            }
                        }
                    }
                }
                for (int i = 0; i < deltagarAttendence.Count; i++)
                {
                    if (i == deltagarAttendence.Count - 1)
                    {
                        var daysToCountFromMonday = 0;
                        if (deltagarAttendence[i].DateConcerning.AddDays(2).Month == deltagarAttendence[i].DateConcerning.Month)
                            daysToCountFromMonday = 1;
                        if (deltagarAttendence[i].DateConcerning.AddDays(3).Month == deltagarAttendence[i].DateConcerning.Month)
                            daysToCountFromMonday = 2;
                        if (deltagarAttendence[i].DateConcerning.AddDays(4).Month == deltagarAttendence[i].DateConcerning.Month)
                            daysToCountFromMonday = 3;
                        if (deltagarAttendence[i].DateConcerning.AddDays(5).Month == deltagarAttendence[i].DateConcerning.Month)
                            daysToCountFromMonday = 4;

                        if(daysToCountFromMonday == 4)
                        {
                            if (deltagarAttendence[i].Fredag == Models.Attendence.AttendenceOption.HeldagMat ||
                       deltagarAttendence[i].Fredag == Models.Attendence.AttendenceOption.HalvdagMat ||
                       deltagarAttendence[i].Fredag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                beställningarAntal++;
                        }
                        if (daysToCountFromMonday >= 3)
                        {
                            if (deltagarAttendence[i].Torsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                       deltagarAttendence[i].Torsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                       deltagarAttendence[i].Torsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                beställningarAntal++;
                        }
                        if (daysToCountFromMonday >= 2)
                        {
                            if (deltagarAttendence[i].Onsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                       deltagarAttendence[i].Onsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                       deltagarAttendence[i].Onsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                beställningarAntal++;
                        }
                        if (daysToCountFromMonday >= 1)
                        {
                            if (deltagarAttendence[i].Tisdag == Models.Attendence.AttendenceOption.HeldagMat ||
                       deltagarAttendence[i].Tisdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                       deltagarAttendence[i].Tisdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                beställningarAntal++;
                        }
                            if (deltagarAttendence[i].Måndag== Models.Attendence.AttendenceOption.HeldagMat ||
                       deltagarAttendence[i].Måndag == Models.Attendence.AttendenceOption.HalvdagMat ||
                       deltagarAttendence[i].Måndag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                                beställningarAntal++;

                    }
                    else
                    {
                        if (deltagarAttendence[i].Måndag == Models.Attendence.AttendenceOption.HeldagMat ||
                            deltagarAttendence[i].Måndag == Models.Attendence.AttendenceOption.HalvdagMat ||
                            deltagarAttendence[i].Måndag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                            beställningarAntal++;
                        if (deltagarAttendence[i].Tisdag == Models.Attendence.AttendenceOption.HeldagMat ||
                           deltagarAttendence[i].Tisdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                           deltagarAttendence[i].Tisdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                            beställningarAntal++;
                        if (deltagarAttendence[i].Onsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                         deltagarAttendence[i].Onsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                         deltagarAttendence[i].Onsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                            beställningarAntal++;
                        if (deltagarAttendence[i].Torsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                           deltagarAttendence[i].Torsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                           deltagarAttendence[i].Torsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                            beställningarAntal++;
                        if (deltagarAttendence[i].Fredag == Models.Attendence.AttendenceOption.HeldagMat ||
                         deltagarAttendence[i].Fredag == Models.Attendence.AttendenceOption.HalvdagMat ||
                         deltagarAttendence[i].Fredag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                            beställningarAntal++;
                    }
                }
                var pris = PrisDb.Find(x => true).FirstOrDefault().Pris;
                var model = new MatListaMonthViewModel
                {
                    Antal = beställningarAntal,
                    DateConcerning = Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-01"),
                    MatId = deltagare.MatId,
                    DeltagarNamn = deltagare.DeltagarNamn,
                    PrisPerMatlåda = pris,
                    TotalKostnad = (pris * beställningarAntal)

                };
                returningList.Add(model);
            }
            return returningList;
        }

        public int GetPris()
        {
            return PrisDb.Find<MatLådePris>(x => true).FirstOrDefault().Pris;
        }
    }
}
