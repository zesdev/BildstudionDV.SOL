using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Jobb;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BildstudionDV.Test
{
    public class JobbTest
    {
        BildstudionDV.BI.Context.BildStudionDVContext context;
        Jobb jobbDb;
        Kund kundDb;
        DelJobb delJobbDb;

        [SetUp]
        public void Setup()
        {
            var appSettingValFromStatic = ConfigurationManager.AppSettings["mySetting"];
            var username = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["username"].Value;
            var password = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["password"].Value;
            context = new BI.Context.BildStudionDVContext(username, password);
            delJobbDb = new DelJobb(context);
            jobbDb = new Jobb(context, delJobbDb);
            kundDb = new Kund(context, jobbDb);
        }
        [Test]
        public void a1TestCreateKund()
        {
            var kundModel = new KundModel { KundNamn = "Bert Karlsson" };
            kundDb.AddKund(kundModel);
            Assert.AreEqual(1, kundDb.GetAllKunder().Count);
        }
        [Test]
        public void a2TestChangeKundNamn()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            kund.KundNamn = "Bert Larsson";
            kundDb.UpdateKund(kund);
            Assert.AreEqual("Bert Larsson", kundDb.GetAllKunder().FirstOrDefault().KundNamn);
        }
        [Test]
        public void a3TestAddJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobbModel = new JobbModel { KundId = kund.Id, StatusPåJobbet = StatusTyp.Påbörjat, DatumRegistrerat = DateTime.Now, Title="Bildjobb", TypAvJobb = JobbTyp.Bilder, TypAvPrioritet = PrioritetTyp.Låg };
            jobbDb.AddJobb(jobbModel);
            Assert.AreEqual(1, jobbDb.GetAllJobbs().Count);
        }
        [Test]
        public void a4TestEditJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            jobb.StatusPåJobbet = StatusTyp.KlarOchAvhämtat;
            jobbDb.UpdateJobb(jobb);
            Assert.AreEqual(StatusTyp.KlarOchAvhämtat, jobbDb.GetAllJobbs().FirstOrDefault().StatusPåJobbet);
        }
        [Test]
        public void a5TestAddDelJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            var delJobbModel = new DelJobbModel { StatusPåJobbet = DelJobbStatus.AttGöras, JobbId = jobb.Id, Namn = "Deljob blabla", VemGör = "Erik" };
            delJobbDb.AddDelJobb(delJobbModel);
            Assert.AreEqual(1, delJobbDb.GetDelJobbsInJobb(jobb.Id).Count);
        }
        [Test]
        public void a6TestRedigeraDeljJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            var delJobbModel = delJobbDb.GetDelJobbsInJobb(jobb.Id).FirstOrDefault();
            delJobbModel.StatusPåJobbet = DelJobbStatus.Klar;
            delJobbDb.UpdateDelJobb(delJobbModel);
            Assert.AreEqual(DelJobbStatus.Klar, delJobbDb.GetDelJobbsInJobb(jobb.Id).FirstOrDefault().StatusPåJobbet);
        }
        [Test]
        public void a7TestTaBortDelJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            var delJobbModel = delJobbDb.GetDelJobbsInJobb(jobb.Id).FirstOrDefault();
            delJobbDb.RemoveDelJobb(delJobbModel.Id);
            Assert.AreEqual(0, delJobbDb.GetDelJobbsInJobb(jobb.Id).Count);
        }
        [Test]
        public void p1TestRemoveJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            jobbDb.RemoveJobb(jobb.Id);
            Assert.AreEqual(0, jobbDb.GetAllJobbs().Count);
        }
        [Test]
        public void q1RemoveKund()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            kundDb.RemoveKund(kund.Id);
            Assert.AreEqual(0, kundDb.GetAllKunder().Count);
        }
    }
}
