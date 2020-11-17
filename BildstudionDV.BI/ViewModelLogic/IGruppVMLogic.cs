using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IGruppVMLogic
    {
        void AddGrupp(GruppViewModel viewModel);
        List<GruppViewModel> GetGrupperInEnhet(ObjectId enhetId);
        List<GruppViewModel> GetAllGrupper();
        void RemoveAllGrupperInEnhet(ObjectId enhetId);
        void RemoveGrupp(ObjectId gruppId);
        void UpdateGrupp(GruppViewModel viewModel);
        
    }
}