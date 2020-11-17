using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Inventarie;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public class InventarieVMLogic : IInventarieVMLogic
    {
        Inventarie inventarieDb;
        public InventarieVMLogic(Inventarie _inventarieDb)
        {
            inventarieDb = _inventarieDb;
        }
        public void AddInventarie(InventarieViewModel viewModel)
        {
            var model = new InventarieModel
            {
                Antal = viewModel.Antal,
                DatumRegistrerat = DateTime.Now,
                Fabrikat = viewModel.Fabrikat,
                GruppId = viewModel.GruppId,
                InventarieKommentar = viewModel.InventarieKommentar,
                InventarieNamn = viewModel.InventarieNamn,
                Pris = viewModel.Pris
            };
            inventarieDb.AddInventarie(model);
        }
        public void RemoveInventarie(ObjectId Id)
        {
            inventarieDb.RemoveInventarie(Id);
        }
        public void RemoveAllInventarieInGrupp(ObjectId gruppId)
        {
            inventarieDb.RemoveAllInventarierInGrupp(gruppId);
        }
        public List<InventarieViewModel> GetInventarierFörGrupp(ObjectId gruppId)
        {
            var returningList = new List<InventarieViewModel>();
            var rawModels = inventarieDb.GetListOfInventarierInGrupp(gruppId);
            foreach (var model in rawModels)
            {
                var viewModel = new InventarieViewModel
                {
                    Antal = model.Antal,
                    DatumRegistrerat = model.DatumRegistrerat,
                    Fabrikat = model.Fabrikat,
                    GruppId = model.GruppId,
                    Id = model.Id,
                    InventarieKommentar = model.InventarieKommentar,
                    InventarieNamn = model.InventarieKommentar,
                    Pris = model.Pris
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        void IInventarieVMLogic.UpdateInventarie(InventarieViewModel inventarie)
        {
            var model = new InventarieModel
            {
                Antal = inventarie.Antal,
                DatumRegistrerat = inventarie.DatumRegistrerat,
                Fabrikat = inventarie.Fabrikat,
                GruppId = inventarie.GruppId,
                Id = inventarie.Id,
                InventarieKommentar = inventarie.InventarieKommentar,
                InventarieNamn = inventarie.InventarieNamn,
                Pris = inventarie.Pris
            };
            inventarieDb.UpdateInventarieItem(model);
        }
    }
}
