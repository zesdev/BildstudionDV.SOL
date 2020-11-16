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
    public class JobbVMLogic : IJobbVMLogic
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
                StatusPåJobbet = viewModel.StatusPåJobbet.ToString(),
                Id = viewModel.Id,
                AccessId = viewModel.AccessId,
                KundId = viewModel.KundId,
                Title = viewModel.Title,
                TypAvJobb = viewModel.TypAvJobb.ToString(),
                TypAvPrioritet = viewModel.TypAvPrioritet.ToString()
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
                    StatusPåJobbet = HelperConvertLogic.GetStatusTypFromString(model.StatusPåJobbet),
                    DatumRegistrerat = model.DatumRegistrerat,
                    delJobbs = delJobbVMLogic.GetDelJobbsInJobb(model.Id),
                    Id = model.Id,
                    AccessId = model.AccessId,
                    KundId = model.KundId,
                    Title = model.Title,
                    TypAvJobb = HelperConvertLogic.GetJobbTypFromString(model.TypAvJobb),
                    TypAvPrioritet = HelperConvertLogic.GetPrioritetTypFromString(model.TypAvPrioritet)
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
                StatusPåJobbet = HelperConvertLogic.GetStatusTypFromString(model.StatusPåJobbet),
                DatumRegistrerat = model.DatumRegistrerat,
                delJobbs = delJobbVMLogic.GetDelJobbsInJobb(model.Id),
                Id = model.Id,
                AccessId = model.AccessId,
                KundId = model.KundId,
                Title = model.Title,
                TypAvJobb = HelperConvertLogic.GetJobbTypFromString(model.TypAvJobb),
                TypAvPrioritet = HelperConvertLogic.GetPrioritetTypFromString(model.TypAvPrioritet)
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

        public void AddJobb(JobbViewModel kundJobb)
        {
            var index = 0;
            try
            {
                var lastJobIndex = jobbDb.GetAllJobbs().LastOrDefault().AccessId;
                index = lastJobIndex + 1;
            }
            catch
            {
            }
            var model = new JobbModel
            {
                StatusPåJobbet = kundJobb.StatusPåJobbet.ToString(),
                DatumRegistrerat = kundJobb.DatumRegistrerat,
                KundId = kundJobb.KundId,
                AccessId = index,
                Title = kundJobb.Title,
                TypAvJobb = kundJobb.TypAvJobb.ToString(),
                TypAvPrioritet = kundJobb.TypAvPrioritet.ToString()
            };
            jobbDb.AddJobb(model);
        }
    }
}
