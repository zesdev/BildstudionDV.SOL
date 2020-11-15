using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.MainLogic
{
    public interface IDeltagarViewLogic
    {
        List<ViewModelDeltagareAttendence> GetAllDeltagareViewData();
        ViewModelDeltagareAttendence GetDeltagareViewData(ObjectId UserId);
    }
}