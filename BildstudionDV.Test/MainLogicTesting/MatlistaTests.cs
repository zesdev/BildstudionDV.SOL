using BildstudionDV.BI.Database;
using BildstudionDV.BI.MainLogic;
using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BildstudionDV.Test.MainLogicTesting
{
    public class MatlistaTests
    {
        BildstudionDV.BI.Context.BildStudionDVContext context;
        Närvaro närvaroDb;
        Deltagare deltagareDb;
        DeltagareVMLogic deltagareVMLogic;
        NärvaroVMLogic närvaroVMLogic;
        MatlistaLogic matlistaLogic;
        DeltagarViewLogic deltagarViewLogic;
        string deltagarNamn = "UTestDeltagareMatListaTest";
        [SetUp]
        public void Setup()
        {
            var appSettingValFromStatic = ConfigurationManager.AppSettings["mySetting"];
            var username = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["username"].Value;
            var password = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["password"].Value;
            context = new BI.Context.BildStudionDVContext(username, password);
            närvaroDb = new Närvaro(context);
            deltagareDb = new Deltagare(context, närvaroDb);
            deltagareVMLogic = new DeltagareVMLogic(deltagareDb);
            närvaroVMLogic = new NärvaroVMLogic(närvaroDb, deltagareDb);
            matlistaLogic = new MatlistaLogic(context, närvaroVMLogic, deltagareVMLogic);
            deltagarViewLogic = new DeltagarViewLogic(deltagareVMLogic, närvaroVMLogic);
        }

        [Test]
        public void A1TestAddDeltagare()
        {
            var deltagarVm = new DeltagareViewModel
            {
                DeltagarNamn = deltagarNamn,
                Måndag = BI.Models.WorkDay.Heldag,
                Tisdag = BI.Models.WorkDay.Heldag,
                Onsdag = BI.Models.WorkDay.Heldag,
                Torsdag = BI.Models.WorkDay.Heldag,
                Fredag = BI.Models.WorkDay.Heldag,
                IsActive = true,
                MatId = "1231532531231"
            };
            deltagareVMLogic.AddDeltagare(deltagarVm);
            Assert.AreEqual(deltagarNamn, deltagareVMLogic.GetAllDeltagare().FirstOrDefault(x => x.DeltagarNamn == deltagarNamn).DeltagarNamn);
        }
        [Test]
        public void A2TestAddAttendences()
        {
            var model = deltagareVMLogic.GetAllDeltagare().FirstOrDefault(x => x.DeltagarNamn == deltagarNamn);
            for (int i = 1; i < 53; i++)
            {
                var attendence = närvaroVMLogic.GetAttendenceForDate(DateTime.Now.AddDays(-(i*7)), model).FirstOrDefault();
                attendence.Måndag = BI.Models.Attendence.AttendenceOption.HeldagMat;
                attendence.Tisdag = BI.Models.Attendence.AttendenceOption.HeldagMat;
                attendence.Onsdag = BI.Models.Attendence.AttendenceOption.HeldagMat;
                attendence.Torsdag = BI.Models.Attendence.AttendenceOption.HeldagMat;
                attendence.Fredag = BI.Models.Attendence.AttendenceOption.HeldagMat;

                närvaroVMLogic.UpdateAttendence(attendence);
            }
            Assert.AreEqual(52, närvaroVMLogic.GetAllAttendence().Where(x => x.DeltagarNamn == deltagarNamn).ToList().Count);
        }
        [Test]
        public void A3TestCheckIfMatListaIsCorrect()
        {
            var deltagare = deltagareVMLogic.GetAllDeltagare().FirstOrDefault(x => x.DeltagarNamn == deltagarNamn);
            for (int i = 1; i < 12; i++)
            {
                var date = DateTime.Now.AddMonths(-i);
                int expectedMatBeställningar = WeekDaysInMonth(date.Year, date.Month);
                var matlistaDeltagare = matlistaLogic.GetAttendenceForMonth(date.Month, date.Year).FirstOrDefault(x => x.DeltagarNamn == deltagarNamn);
                Assert.AreEqual(expectedMatBeställningar ,matlistaDeltagare.Antal);
            }
        }
        [Test]
        public void z1TestRemoveDeltagare()
        {
            var deltagare = deltagareVMLogic.GetAllDeltagare().FirstOrDefault(x => x.DeltagarNamn == deltagarNamn);
            deltagareVMLogic.RemoveDeltagare(deltagare.Id);
        }
        public static int WeekDaysInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            List<DateTime> dates = new List<DateTime>();
            for (int i = 1; i <= days; i++)
            {
                dates.Add(new DateTime(year, month, i));
            }

            int weekDays = dates.Where(d => d.DayOfWeek > DayOfWeek.Sunday & d.DayOfWeek < DayOfWeek.Saturday).Count();
            return weekDays;
        }
    }
}
