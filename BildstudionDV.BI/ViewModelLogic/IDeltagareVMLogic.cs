using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IDeltagareVMLogic
    {
        void AddDeltagare(DeltagareViewModel viewModel);
        List<DeltagareViewModel> GetAllDeltagare();
        DeltagareViewModel GetDeltagare(ObjectId Id);
        void RemoveDeltagare(ObjectId Id);
        void UpdateDeltagare(DeltagareViewModel viewModel);
    }
}