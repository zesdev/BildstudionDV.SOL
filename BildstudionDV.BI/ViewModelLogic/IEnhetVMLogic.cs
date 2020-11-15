using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IEnhetVMLogic
    {
        void AddEnhet(EnhetViewModel viewModel);
        List<EnhetViewModel> GetAllEnheter();
        EnhetViewModel GetEnhet(ObjectId Id);
        void RemoveEnhet(ObjectId enhetId);
        void UpdateEnhet(EnhetViewModel viewModel);
    }
}