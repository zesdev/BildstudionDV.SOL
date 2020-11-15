using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Inventarie;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public class GruppVMLogic : IGruppVMLogic
    {
        Grupp gruppDb;
        InventarieVMLogic inventarieVMLogic;
        public GruppVMLogic(Grupp _gruppDb, InventarieVMLogic _inventarieVMLogic)
        {
            inventarieVMLogic = _inventarieVMLogic;
            gruppDb = _gruppDb;
        }
        public void AddGrupp(GruppViewModel viewModel)
        {
            var model = new GruppModel
            {
                EnhetId = viewModel.EnhetId,
                GruppNamn = viewModel.GruppNamn
            };
            gruppDb.AddGrupp(model);
        }
        public void UpdateGrupp(GruppViewModel viewModel)
        {
            var model = new GruppModel
            {
                EnhetId = viewModel.EnhetId,
                GruppNamn = viewModel.GruppNamn,
                Id = viewModel.Id
            };
            gruppDb.UpdateGruppModel(model);
        }
        public void RemoveGrupp(ObjectId gruppId)
        {
            gruppDb.RemoveGrupp(gruppId);
        }
        public void RemoveAllGrupperInEnhet(ObjectId enhetId)
        {
            gruppDb.RemoveAllGruppsInEnhet(enhetId);
        }
        public List<GruppViewModel> GetGrupperInEnhet(ObjectId enhetId)
        {
            var returningList = new List<GruppViewModel>();
            var rawModels = gruppDb.GetAllGruppsInEnhet(enhetId);
            foreach (var model in rawModels)
            {
                var viewModel = new GruppViewModel
                {
                    EnhetId = model.EnhetId,
                    GruppNamn = model.GruppNamn,
                    Id = model.Id,
                    InventarierInGrupp = inventarieVMLogic.GetInventarierFörGrupp(model.Id)
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
    }
}
