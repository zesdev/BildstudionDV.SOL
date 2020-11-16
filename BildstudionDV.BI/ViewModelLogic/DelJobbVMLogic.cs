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
    public class DelJobbVMLogic : IDelJobbVMLogic
    {
        DelJobb delJobbDb;
        public DelJobbVMLogic(DelJobb _delJobbDb)
        {
            delJobbDb = _delJobbDb;
        }
        public void UpdateDelJobb(DelJobbViewModel viewModel)
        {
            var model = new DelJobbModel
            {
                Id = viewModel.Id,
                AccessId = viewModel.AccessId,
                StatusPåJobbet = viewModel.StatusPåJobbet.ToString(),
                JobbId = viewModel.JobbId,
                Namn = viewModel.Namn,
                VemGör = viewModel.VemGör
            };
            delJobbDb.UpdateDelJobb(model);
        }
        public List<DelJobbViewModel> GetDelJobbsInJobb(ObjectId jobbId)
        {
            var returningList = new List<DelJobbViewModel>();
            var rawModels = delJobbDb.GetDelJobbsInJobb(jobbId);
            foreach (var model in rawModels)
            {
                var viewModel = new DelJobbViewModel
                {
                    StatusPåJobbet = HelperConvertLogic.GetDelJobbStatusFromString(model.StatusPåJobbet),
                    Id = model.Id,
                    AccessId = model.AccessId,
                    JobbId = jobbId,
                    Namn = model.Namn,
                    VemGör = model.VemGör
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        public void AddDelJobb(DelJobbViewModel viewModel)
        {
            int index = 0;
            try
            {
                var lastJobIndex = delJobbDb.GetAllDelJobbs().LastOrDefault().AccessId;
                index = lastJobIndex + 1;
            }
            catch
            {
            }
            var model = new DelJobbModel
            {
                StatusPåJobbet = viewModel.StatusPåJobbet.ToString(),
                JobbId = viewModel.JobbId,
                AccessId = index,
                Namn = viewModel.Namn,
                VemGör = viewModel.VemGör
            };
            delJobbDb.AddDelJobb(model);
        }
        public void RemoveDelJobb(ObjectId delJobbId)
        {
            delJobbDb.RemoveDelJobb(delJobbId);
        }
        public void RemoveAllDelJobbsInJobb(ObjectId jobbId)
        {
            delJobbDb.RemoveAllDelJobbsInJobb(jobbId);
        }
    }
}
