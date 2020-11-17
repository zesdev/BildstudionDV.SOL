using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IInventarieVMLogic
    {
        void AddInventarie(InventarieViewModel viewModel);
        List<InventarieViewModel> GetInventarierFörGrupp(ObjectId gruppId);
        void RemoveAllInventarieInGrupp(ObjectId gruppId);
        void RemoveInventarie(ObjectId Id);
        void UpdateInventarie(InventarieViewModel inventarie);
    }
}