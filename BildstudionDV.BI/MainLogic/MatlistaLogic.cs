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
                var deltagarAttendence = attendenceData.Where(x => x.DeltagarIdInQuestion == deltagare.Id).ToList();
                foreach (var attendenceItem in deltagarAttendence)
                {
                    if (attendenceItem.Måndag == Models.Attendence.AttendenceOption.HeldagMat ||
                        attendenceItem.Måndag == Models.Attendence.AttendenceOption.HalvdagMat ||
                        attendenceItem.Måndag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                        beställningarAntal++;
                    if (attendenceItem.Tisdag == Models.Attendence.AttendenceOption.HeldagMat ||
                       attendenceItem.Tisdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                       attendenceItem.Tisdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                        beställningarAntal++;
                    if (attendenceItem.Onsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                     attendenceItem.Onsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                     attendenceItem.Onsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                        beställningarAntal++;
                    if (attendenceItem.Torsdag == Models.Attendence.AttendenceOption.HeldagMat ||
                       attendenceItem.Torsdag == Models.Attendence.AttendenceOption.HalvdagMat ||
                       attendenceItem.Torsdag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                        beställningarAntal++;
                    if (attendenceItem.Fredag == Models.Attendence.AttendenceOption.HeldagMat ||
                     attendenceItem.Fredag == Models.Attendence.AttendenceOption.HalvdagMat ||
                     attendenceItem.Fredag == Models.Attendence.AttendenceOption.FrånvarandeMat)
                        beställningarAntal++;
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
    }
}
