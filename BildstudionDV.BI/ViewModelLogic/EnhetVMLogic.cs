using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Inventarie;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public class EnhetVMLogic : IEnhetVMLogic
    {
        Enhet enhetDb;
        GruppVMLogic gruppVMLogic;
        public EnhetVMLogic(Enhet _enhetDb, GruppVMLogic _gruppVMLogic)
        {
            enhetDb = _enhetDb;
            gruppVMLogic = _gruppVMLogic;
        }
        public void AddEnhet(EnhetViewModel viewModel)
        {
            var model = new EnhetModel
            {
                ChefNamn = viewModel.ChefNamn,
                Namn = viewModel.Namn
            };
            enhetDb.AddEnhet(model);
        }
        public void RemoveEnhet(ObjectId enhetId)
        {
            enhetDb.RemoveEnhet(enhetId);
        }
        public void UpdateEnhet(EnhetViewModel viewModel)
        {
            var model = new EnhetModel
            {
                ChefNamn = viewModel.ChefNamn,
                Id = viewModel.Id,
                Namn = viewModel.Namn
            };
            enhetDb.UpdateEnhet(model);
        }
        public List<EnhetViewModel> GetAllEnheter()
        {
            var returningList = new List<EnhetViewModel>();
            var rawModels = enhetDb.GetAllEnheter();
            foreach (var model in rawModels)
            {
                var viewModel = new EnhetViewModel
                {
                    ChefNamn = model.ChefNamn,
                    grupperInEnhet = gruppVMLogic.GetGrupperInEnhet(model.Id),
                    Id = model.Id,
                    Namn = model.Namn
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        public EnhetViewModel GetEnhet(ObjectId Id)
        {
            var model = enhetDb.GetEnhet(Id);
            var viewModel = new EnhetViewModel
            {
                ChefNamn = model.ChefNamn,
                grupperInEnhet = gruppVMLogic.GetGrupperInEnhet(model.Id),
                Id = model.Id,
                Namn = model.Namn
            };
            return viewModel;
        }
    }
}
