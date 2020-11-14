using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models.Jobb;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public class JobbVMLogic
    {
        Jobb jobbDb;
        DelJobbVMLogic delJobbVMLogic;
        public JobbVMLogic(Jobb _jobbDb, DelJobbVMLogic _deljobbVMLogic)
        {
            jobbDb = _jobbDb;
            delJobbVMLogic = _deljobbVMLogic;
        }
        public void UpdateJobb(JobbViewModel viewModel)
        {
            var model = new JobbModel
            {
                DatumRegistrerat = viewModel.DatumRegistrerat,
                StatusPåJobbet = viewModel.StatusPåJobbet,
                Id = viewModel.Id,
                KundId = viewModel.KundId,
                Title = viewModel.Title,
                TypAvJobb = viewModel.TypAvJobb,
                TypAvPrioritet = viewModel.TypAvPrioritet
            };
            jobbDb.UpdateJobb(model);
        }
        public List<JobbViewModel> GetJobbsForKund(ObjectId kundId)
        {
            var returningList = new List<JobbViewModel>();
            var rawModels = jobbDb.GetAllJobbs().Where(x => x.KundId == kundId).ToList();
            foreach (var model in rawModels)
            {
                var viewModel = new JobbViewModel
                {
                    StatusPåJobbet = model.StatusPåJobbet,
                    DatumRegistrerat = model.DatumRegistrerat,
                    delJobbs = delJobbVMLogic.GetDelJobbsInJobb(model.Id),
                    Id = model.Id,
                    KundId = model.KundId,
                    Title = model.Title,
                    TypAvJobb = model.TypAvJobb,
                    TypAvPrioritet = model.TypAvPrioritet
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        public JobbViewModel GetJobbViewModel(ObjectId jobbId)
        {
            var model = jobbDb.GetJobb(jobbId);
            var viewModel = new JobbViewModel
            {
                StatusPåJobbet = model.StatusPåJobbet,
                DatumRegistrerat = model.DatumRegistrerat,
                delJobbs = delJobbVMLogic.GetDelJobbsInJobb(model.Id),
                Id = model.Id,
                KundId = model.KundId,
                Title = model.Title,
                TypAvJobb = model.TypAvJobb,
                TypAvPrioritet = model.TypAvPrioritet
            };
            return viewModel;
        }
        public void RemoveJobb(ObjectId jobbId)
        {
            jobbDb.RemoveJobb(jobbId);
        }
        public void RemoveAllJobsInKund(ObjectId kundId)
        {
            jobbDb.RemoveAllJobbsFörKund(kundId);
        }
    }
}
