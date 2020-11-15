using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IDelJobbVMLogic
    {
        void AddDelJobb(DelJobbViewModel viewModel);
        List<DelJobbViewModel> GetDelJobbsInJobb(ObjectId jobbId);
        void RemoveAllDelJobbsInJobb(ObjectId jobbId);
        void RemoveDelJobb(ObjectId delJobbId);
        void UpdateDelJobb(DelJobbViewModel viewModel);
    }
}