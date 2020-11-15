using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IKundVMLogic
    {
        void AddKund(KundViewModel viewModel);
        KundViewModel GetKund(ObjectId kundId);
        List<KundViewModel> GetKunder();
        void RemoveKund(ObjectId kundId);
        void UpdateKund(KundViewModel viewModel);
    }
}