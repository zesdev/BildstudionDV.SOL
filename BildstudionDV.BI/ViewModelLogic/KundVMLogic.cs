using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Jobb;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public class KundVMLogic
    {
        Kund kundDb;
        JobbVMLogic jobbVMLogic;
        public KundVMLogic(Kund _kundDb, JobbVMLogic _jobbVMLogic)
        {
            kundDb = _kundDb;
            jobbVMLogic = _jobbVMLogic;
        }
        public List<KundViewModel> GetKunder()
        {
            var returningList = new List<KundViewModel>();
            var rawModels = kundDb.GetAllKunder();
            foreach (var model in rawModels)
            {
                var viewModel = new KundViewModel
                {
                    Id = model.Id,
                    KundNamn = model.KundNamn,
                    listOfJobbs = jobbVMLogic.GetJobbsForKund(model.Id)
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        public KundViewModel GetKund(ObjectId kundId)
        {
            var model = kundDb.GetKund(kundId);
            var viewModel = new KundViewModel
            {
                Id = model.Id,
                KundNamn = model.KundNamn,
                listOfJobbs = jobbVMLogic.GetJobbsForKund(model.Id)
            };
            return viewModel;
        }
        public void RemoveKund(ObjectId kundId)
        {
            kundDb.RemoveKund(kundId);
        }
        public void UpdateKund(KundViewModel viewModel)
        {
            var model = new KundModel
            {
                Id = viewModel.Id,
                KundNamn = viewModel.KundNamn
            };
            kundDb.UpdateKund(model);
        }
        public void AddKund(KundViewModel viewModel)
        {
            var model = new KundModel
            {
                KundNamn = viewModel.KundNamn
            };
            kundDb.AddKund(model);
        }
    }
}
