using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models;
using BildstudionDV.BI.Models.Attendence;
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

        [SetUp]
        public void Setup()
        {
            var appSettingValFromStatic = ConfigurationManager.AppSettings["mySetting"];
            var username = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["username"].Value;
            var password = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["password"].Value;
            context = new BI.Context.BildStudionDVContext(username, password);
            närvaroDb = new Närvaro(context);
            deltagareDb = new Deltagare(context);
        }
        [Test]
        public void a1TestLäggTillDeltagare()
        {
            var deltagarModel = new DeltagareModel { DeltagarNamn = "Erik", IsActive = true, Måndag = WorkDay.Heldag, Tisdag = WorkDay.Heldag, Onsdag = WorkDay.Heldag, Torsdag = WorkDay.Heldag, Fredag = WorkDay.Halvdag };
            deltagareDb.AddDeltagare(deltagarModel);
            Assert.AreEqual(1, deltagareDb.GetAllDeltagarModels().Count);
        }
        [Test]
        public void a2TestRedigeraDeltagare()
        {
            var deltagare = deltagareDb.GetAllDeltagarModels().FirstOrDefault();
            deltagare.Måndag = WorkDay.Halvdag;
            deltagareDb.UpdateDeltagare(deltagare);
            Assert.AreEqual(WorkDay.Halvdag, deltagareDb.GetAllDeltagarModels().FirstOrDefault().Måndag);
        }
        [Test]
        public void a3TestAddAttendence()
        {
            var deltagare = deltagareDb.GetAllDeltagarModels().FirstOrDefault();
            var attendenceModel = new AttendenceModel { 
                DateConcerning = DateTime.Now, DeltagarIdInQuestion = deltagare.Id, 
                ExpectedMåndag = deltagare.Måndag, ExpectedTisdag = deltagare.Tisdag, ExpectedOnsdag = deltagare.Onsdag, ExpectedTorsdag = deltagare.Torsdag, ExpectedFredag = deltagare.Fredag,
                Måndag = AttendenceOption.HalvdagMat, Tisdag = AttendenceOption.Heldag, Onsdag = AttendenceOption.Heldag, Torsdag = AttendenceOption.Heldag, Fredag = AttendenceOption.Halvdag };
            närvaroDb.AddAttendence(attendenceModel);
            Assert.AreEqual(AttendenceOption.HalvdagMat, närvaroDb.GetAllAttendenceItems().FirstOrDefault().Måndag);
        }
        [Test]
        public void q1TestTaBortDeltagare()
        {
            var deltagarModel = deltagareDb.GetAllDeltagarModels().FirstOrDefault();
            deltagareDb.RemoveDeltagare(deltagarModel.Id);
            Assert.AreEqual(0, deltagareDb.GetAllDeltagarModels().Count);
        }
    }
}
