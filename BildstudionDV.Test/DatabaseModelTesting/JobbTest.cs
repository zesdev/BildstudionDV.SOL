using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Jobb;
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
    public class JobbTest
    {
        BildstudionDV.BI.Context.BildStudionDVContext context;
        Jobb jobbDb;
        Kund kundDb;
        DelJobb delJobbDb;
        KundVMLogic kundVmLogic;
        JobbVMLogic jobbVmLogic;
        DelJobbVMLogic delJobbVMLogic;

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
            delJobbVMLogic = new DelJobbVMLogic(delJobbDb);
            jobbVmLogic = new JobbVMLogic(jobbDb, delJobbVMLogic);
            kundVmLogic = new KundVMLogic(kundDb, jobbVmLogic);
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
            var jobbModel = new JobbModel { KundId = kund.Id, StatusPåJobbet = StatusTyp.Påbörjat.ToString(), DatumRegistrerat = DateTime.Now, Title="Bildjobb", TypAvJobb = JobbTyp.Bilder.ToString(), TypAvPrioritet = PrioritetTyp.Låg.ToString() };
            jobbDb.AddJobb(jobbModel);
            Assert.AreEqual(1, jobbDb.GetAllJobbs().Count);
        }
        [Test]
        public void a4TestEditJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            jobb.StatusPåJobbet = StatusTyp.KlarOchAvhämtat.ToString();
            jobbDb.UpdateJobb(jobb);
            Assert.AreEqual(StatusTyp.KlarOchAvhämtat.ToString(), jobbDb.GetAllJobbs().FirstOrDefault().StatusPåJobbet);
        }
        [Test]
        public void a5TestAddDelJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            var delJobbModel = new DelJobbModel { StatusPåJobbet = DelJobbStatus.AttGöras.ToString(), JobbId = jobb.Id, Namn = "Deljob blabla", VemGör = "Erik" };
            delJobbDb.AddDelJobb(delJobbModel);
            Assert.AreEqual(1, delJobbDb.GetDelJobbsInJobb(jobb.Id).Count);
        }
        [Test]
        public void a6TestRedigeraDeljJobb()
        {
            var kund = kundDb.GetAllKunder().FirstOrDefault();
            var jobb = jobbDb.GetAllJobbs().FirstOrDefault();
            var delJobbModel = delJobbDb.GetDelJobbsInJobb(jobb.Id).FirstOrDefault();
            delJobbModel.StatusPåJobbet = DelJobbStatus.Klar.ToString();
            delJobbDb.UpdateDelJobb(delJobbModel);
            Assert.AreEqual(DelJobbStatus.Klar.ToString(), delJobbDb.GetDelJobbsInJobb(jobb.Id).FirstOrDefault().StatusPåJobbet);
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
        public void b1TestAddKundV()
        {
            var kundVm = new KundViewModel()
            {
                KundNamn = "Uffe Larsson"
            };
            kundVmLogic.AddKund(kundVm);
            Assert.AreEqual(1, kundVmLogic.GetKunder().Where(x => x.KundNamn == "Uffe Larsson").ToList().Count);
        }
        [Test]
        public void b2TestBytKundNamnVM()
        {
            var kundVm = kundVmLogic.GetKunder().FirstOrDefault(x => x.KundNamn == "Uffe Larsson");
            kundVm.KundNamn = "Nils Larsson";
            kundVmLogic.UpdateKund(kundVm);
            Assert.AreEqual("Nils Larsson", kundVmLogic.GetKunder().FirstOrDefault(x => x.Id == kundVm.Id).KundNamn);
        }
        [Test]
        public void b3AddJobbVM()
        {
            var kundVm = kundVmLogic.GetKunder().FirstOrDefault(x => x.KundNamn == "Nils Larsson");
            var kundJobb = new JobbViewModel
            {
                StatusPåJobbet = StatusTyp.Påbörjat,
                DatumRegistrerat = DateTime.Now,
                KundId = kundVm.Id,
                Title = "Viktigt bildjobb",
                TypAvJobb = JobbTyp.Bilder,
                TypAvPrioritet = PrioritetTyp.Hög
            };
            jobbVmLogic.AddJobb(kundJobb);
            Assert.AreEqual(1, jobbVmLogic.GetJobbsForKund(kundVm.Id).Count);
        }

        [Test]
        public void d1TestRemoveKundVm()
        {
            var kundVm = kundVmLogic.GetKunder().FirstOrDefault(x => x.KundNamn == "Nils Larsson");
            kundVmLogic.RemoveKund(kundVm.Id);
            Assert.AreEqual(1, kundVmLogic.GetKunder().Count);
        }
        [Test]
        public void b4EditJobbVm()
        {
            var kundVm = kundVmLogic.GetKunder().FirstOrDefault(x => x.KundNamn == "Nils Larsson");
            var kundJobb = jobbVmLogic.GetJobbsForKund(kundVm.Id).FirstOrDefault();
            kundJobb.StatusPåJobbet = StatusTyp.SkaKollas;
            jobbVmLogic.UpdateJobb(kundJobb);
            var kundJobbrefreshed = jobbVmLogic.GetJobbsForKund(kundVm.Id).FirstOrDefault();
            Assert.AreEqual(StatusTyp.SkaKollas ,kundJobbrefreshed.StatusPåJobbet);
        }
        [Test]
        public void b5TestAddDeljobb()
        {
            var kundVm = kundVmLogic.GetKunder().FirstOrDefault(x => x.KundNamn == "Nils Larsson");
            var kundJobb = jobbVmLogic.GetJobbsForKund(kundVm.Id).FirstOrDefault();
            var delJobbVM = new DelJobbViewModel
            {
                StatusPåJobbet = DelJobbStatus.AttGöras,
                JobbId = kundJobb.Id,
                Namn = "Diabilder",
                VemGör = "Erik"
            };
            delJobbVMLogic.AddDelJobb(delJobbVM);
            var deljobb = delJobbVMLogic.GetDelJobbsInJobb(kundJobb.Id).FirstOrDefault();
            Assert.AreEqual("Diabilder", deljobb.Namn);
        }
        [Test]
        public void b6TestEditDeljobb()
        {
            var kundVm = kundVmLogic.GetKunder().FirstOrDefault(x => x.KundNamn == "Nils Larsson");
            var kundJobb = jobbVmLogic.GetJobbsForKund(kundVm.Id).FirstOrDefault();
            var delJobbVM = delJobbVMLogic.GetDelJobbsInJobb(kundJobb.Id).FirstOrDefault();
            delJobbVM.StatusPåJobbet = DelJobbStatus.Klar;
            delJobbVMLogic.UpdateDelJobb(delJobbVM);
            var deljobb = delJobbVMLogic.GetDelJobbsInJobb(kundJobb.Id).FirstOrDefault();
            Assert.AreEqual(DelJobbStatus.Klar, deljobb.StatusPåJobbet);
        }
        [Test]
        public void b7TestRemoveDeljobb()
        {
            var kundVm = kundVmLogic.GetKunder().FirstOrDefault(x => x.KundNamn == "Nils Larsson");
            var kundJobb = jobbVmLogic.GetJobbsForKund(kundVm.Id).FirstOrDefault();
            var delJobbVM = delJobbVMLogic.GetDelJobbsInJobb(kundJobb.Id).FirstOrDefault();
            delJobbVMLogic.RemoveDelJobb(delJobbVM.Id);
            Assert.AreEqual(0, delJobbVMLogic.GetDelJobbsInJobb(kundJobb.Id).Count);
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
