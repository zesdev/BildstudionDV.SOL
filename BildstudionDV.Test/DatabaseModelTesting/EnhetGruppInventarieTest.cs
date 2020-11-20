using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Inventarie;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BildstudionDV.Test
{
    class EnhetGruppInventarieTest
    {
        BildstudionDV.BI.Context.BildStudionDVContext context;
        Enhet enhetDb;
        Grupp gruppDb;
        Inventarie inventarieDb;
        string enhetNamn;
        [SetUp]
        public void Setup()
        {
            var appSettingValFromStatic = ConfigurationManager.AppSettings["mySetting"];
            var username = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["username"].Value;
            var password = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["password"].Value;
            context = new BI.Context.BildStudionDVContext(username, password);
            inventarieDb = new Inventarie(context);
            gruppDb = new Grupp(context, inventarieDb);
            enhetDb = new Enhet(context, gruppDb);
        }
        [Test]
        public void a1TestAddEnhet()
        {
            var precount = enhetDb.GetAllEnheter().Count;
            enhetNamn = "Enhet 404";
            enhetDb.AddEnhet(new BI.Models.Inventarie.EnhetModel { ChefNamn = "Bert Karlsson", Namn = enhetNamn });
            Assert.AreEqual(precount+1, enhetDb.GetAllEnheter().Count);
        }
        [Test]
        public void a2TestUpdateEnhetNamn()
        {
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            enhetNamn = "Enhet 404 bytt namn";
            enhet.Namn = enhetNamn;
            
            enhetDb.UpdateEnhet(enhet);
            Assert.AreEqual("Enhet 404 bytt namn", enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn).Namn);
        }
        [Test]
        public void a3TestAddGruppToEnhet()
        {
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            var gruppModel = new GruppModel { EnhetId = enhet.Id, GruppNamn = "Bildstudion" };
            var precount = gruppDb.GetAllGruppsInEnhet(enhet.Id).Count;
            gruppDb.AddGrupp(gruppModel);
            Assert.AreEqual(precount+1, gruppDb.GetAllGruppsInEnhet(enhet.Id).Count);
        }
        [Test]
        public void a4TestBytNamnPåGrupp()
        {
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            var gruppModel = gruppDb.GetAllGruppsInEnhet(enhet.Id).FirstOrDefault();
            gruppModel.GruppNamn = "Bildstudion bytt namn";
            gruppDb.UpdateGruppModel(gruppModel);
            enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            var gruppNamnUpdated = gruppDb.GetAllGruppsInEnhet(enhet.Id).First().GruppNamn;
            Assert.AreEqual("Bildstudion bytt namn", gruppNamnUpdated);
        }

        [Test]
        public void a5TestAddInventarie()
        {
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            var grupp = gruppDb.GetAllGruppsInEnhet(enhet.Id).FirstOrDefault();
            var inventarieModel = new InventarieModel { Antal="1", Fabrikat= "Toshiba", GruppId = grupp.Id, InventarieKommentar="jättestark dator", InventarieNamn="Dator", Pris= "20kr" };
            inventarieDb.AddInventarie(inventarieModel);
            Assert.AreEqual(1, inventarieDb.GetListOfInventarierInGrupp(grupp.Id).Count);
        }
        [Test]
        public void a6TestEditInventarie()
        {
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            var grupp = gruppDb.GetAllGruppsInEnhet(enhet.Id).FirstOrDefault();
            var inventarie = inventarieDb.GetListOfInventarierInGrupp(grupp.Id).FirstOrDefault();
            inventarie.InventarieNamn = "Gammal dator";
            inventarieDb.UpdateInventarieItem(inventarie);
            Assert.AreEqual("Gammal dator", inventarieDb.GetListOfInventarierInGrupp(grupp.Id).FirstOrDefault().InventarieNamn);
        }
        [Test]
        public void u1TestTaBortInventarie()
        {
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            var grupp = gruppDb.GetAllGruppsInEnhet(enhet.Id).FirstOrDefault();
            var inventarie = inventarieDb.GetListOfInventarierInGrupp(grupp.Id).FirstOrDefault();
            inventarieDb.RemoveInventarie(inventarie.Id);
            Assert.AreEqual(0, inventarieDb.GetListOfInventarierInGrupp(grupp.Id).Count);
        }
        [Test]
        public void x1TestTaBortGrupp()
        {
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            var grupp = gruppDb.GetAllGruppsInEnhet(enhet.Id).FirstOrDefault();
            gruppDb.RemoveGrupp(grupp.Id);
        }
        [Test]
        public void y1RemoveEnhet()
        {
            var precount = enhetDb.GetAllEnheter().Count;
            var enhet = enhetDb.GetAllEnheter().FirstOrDefault(x => x.Namn == enhetNamn);
            enhetDb.RemoveEnhet(enhet.Id);
            Assert.AreEqual(precount-1, enhetDb.GetAllEnheter().Count);
        }
    }
}
