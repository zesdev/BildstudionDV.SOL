using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models;
using BildstudionDV.BI.Models.Attendence;
using BildstudionDV.BI.ViewModelLogic;
using BildstudionDV.BI.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BildstudionDV.Test
{
    public class NärvaroTest
    {
        BildstudionDV.BI.Context.BildStudionDVContext context;
        Närvaro närvaroDb;
        Deltagare deltagareDb;
        DeltagareVMLogic deltagareVMLogic;
        NärvaroVMLogic närvaroVMLogic;
        

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
        }
        [Test]
        public void a1TestLäggTillDeltagare()
        {
            var deltagarModel = new DeltagareModel { DeltagarNamn = "Erik", IsActive = true, Måndag = WorkDay.Heldag.ToString(), Tisdag = WorkDay.Heldag.ToString(), Onsdag = WorkDay.Heldag.ToString(), Torsdag = WorkDay.Heldag.ToString(), Fredag = WorkDay.Halvdag.ToString() };
            deltagareDb.AddDeltagare(deltagarModel);
            Assert.AreEqual(1, deltagareDb.GetAllDeltagarModels().Count);
        }
        [Test]
        public void a2TestRedigeraDeltagare()
        {
            var deltagare = deltagareDb.GetAllDeltagarModels().FirstOrDefault();
            deltagare.Måndag = WorkDay.Halvdag.ToString();
            deltagareDb.UpdateDeltagare(deltagare);
            Assert.AreEqual(WorkDay.Halvdag.ToString(), deltagareDb.GetAllDeltagarModels().FirstOrDefault().Måndag);
        }
        [Test]
        public void a3TestAddAttendence()
        {
            var deltagare = deltagareDb.GetAllDeltagarModels().FirstOrDefault();
            var attendenceModel = new AttendenceModel { 
                DateConcerning = DateTime.Now, DeltagarIdInQuestion = deltagare.Id, 
                ExpectedMåndag = deltagare.Måndag, ExpectedTisdag = deltagare.Tisdag, ExpectedOnsdag = deltagare.Onsdag, ExpectedTorsdag = deltagare.Torsdag, ExpectedFredag = deltagare.Fredag,
                Måndag = AttendenceOption.HalvdagMat.ToString(), Tisdag = AttendenceOption.Heldag.ToString(), Onsdag = AttendenceOption.Heldag.ToString(), Torsdag = AttendenceOption.Heldag.ToString(), Fredag = AttendenceOption.Halvdag.ToString()
            };
            närvaroDb.AddAttendence(attendenceModel);
            Assert.AreEqual(AttendenceOption.HalvdagMat.ToString(), närvaroDb.GetAllAttendenceItems().FirstOrDefault().Måndag);
        }
        [Test]
        public void a4TestEditAttendence()
        {
            var attendenceItem = närvaroDb.GetAllAttendenceItems().FirstOrDefault();
            attendenceItem.Måndag = AttendenceOption.HeldagMat.ToString();
            närvaroDb.UpdateAttendence(attendenceItem);
            Assert.AreEqual(AttendenceOption.HeldagMat.ToString(), närvaroDb.GetAttendenceItem(attendenceItem.Id).Måndag);
        }
        [Test]
        public void a5TestTaBortDeltagare()
        {
            var deltagarModel = deltagareDb.GetAllDeltagarModels().FirstOrDefault();
            deltagareDb.RemoveDeltagare(deltagarModel.Id);
            Assert.AreEqual(0, deltagareDb.GetAllDeltagarModels().Count);
        }
        [Test]
        public void b5TestAddDeltagare()
        {
            var viewModel = new DeltagareViewModel
            {
                DeltagarNamn = "Lina",
                Måndag = WorkDay.Halvdag,
                Tisdag = WorkDay.Halvdag,
                Onsdag = WorkDay.Halvdag,
                Torsdag = WorkDay.Halvdag,
                Fredag = WorkDay.Halvdag,
                IsActive = true
            };
            deltagareVMLogic.AddDeltagare(viewModel);
            Assert.AreEqual(1, deltagareVMLogic.GetAllDeltagare().Count);
        }
        [Test]
        public void b6TestUpdateDeltagare()
        {
            var deltagare = deltagareVMLogic.GetAllDeltagare().FirstOrDefault(x => x.DeltagarNamn == "Lina");
            deltagare.Måndag = WorkDay.Heldag;

            deltagareVMLogic.UpdateDeltagare(deltagare);
            Assert.AreEqual(WorkDay.Heldag, deltagareVMLogic.GetAllDeltagare().First(x => x.DeltagarNamn == "Lina").Måndag);
        }
        [Test]
        public void b7AddAttendence()
        {
            var deltagare = deltagareVMLogic.GetAllDeltagare().First(x => x.DeltagarNamn == "Lina");

            var viewModels = närvaroVMLogic.GetAttendenceForDate(DateTime.Now);
            Assert.AreEqual(1, viewModels.Count);
            var linaAttendence = viewModels.First(x => x.DeltagarIdInQuestion == deltagare.Id);
            var indexOfLina = viewModels.IndexOf(linaAttendence);
            viewModels[indexOfLina].Måndag = AttendenceOption.HeldagMat;
            närvaroVMLogic.UpdateAttendences(viewModels);
            var AttendenceItems = närvaroVMLogic.GetAttendenceForDate(DateTime.Now);
            Assert.AreEqual(1, AttendenceItems.Count);
            Assert.AreEqual(AttendenceOption.HeldagMat, AttendenceItems.First(x => x.DeltagarIdInQuestion == deltagare.Id).Måndag);
        }
        [Test]
        public void b8AddAttendence2()
        {
            var lina = deltagareVMLogic.GetAllDeltagare().First(x => x.DeltagarNamn == "Lina");
            var attendenceVms = närvaroVMLogic.GetAttendenceForDate(DateTime.Now.AddDays(7));
            var attendenceForLina = attendenceVms.First(x => x.DeltagarIdInQuestion == lina.Id);
            var indexOfLina = attendenceVms.IndexOf(attendenceForLina);
            attendenceVms[indexOfLina].Tisdag = AttendenceOption.Halvdag;
            närvaroVMLogic.UpdateAttendences(attendenceVms);
            attendenceVms = närvaroVMLogic.GetAttendenceForDate(DateTime.Now.AddDays(7));
            Assert.AreEqual(AttendenceOption.Halvdag, attendenceVms.First(x => x.DeltagarIdInQuestion == lina.Id).Tisdag);
        }
        [Test]
        public void p1TestRemoveDeltagare()
        {
            var lina = deltagareVMLogic.GetAllDeltagare().First(x => x.DeltagarNamn == "Lina");
            deltagareVMLogic.RemoveDeltagare(lina.Id);
            Assert.AreEqual(0, deltagareVMLogic.GetAllDeltagare().Count);
        }
   
    }
}
