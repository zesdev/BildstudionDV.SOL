using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace BildstudionDV.BI.MainLogic
{
    public interface IDeltagarViewLogic
    {
        List<ViewModelDeltagareAttendence> GetAllDeltagareViewData();
        ViewModelDeltagareAttendence GetDeltagareViewData(ObjectId UserId);
        ViewModelMonthAttendence GetAttendenceForDeltagareForMonth(int monthsToLookBack, DateTime dateTime, DeltagareViewModel deltagarn);
        List<ViewModelMonthAttendence> GetMonthAttendenceForDeltagare(ObjectId UserId, int monthstolookback);
    }
}