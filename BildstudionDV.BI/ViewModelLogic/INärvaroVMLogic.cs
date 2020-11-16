using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface INärvaroVMLogic
    {
        void AddNärvaro(AttendenceViewModel viewModel);
        List<AttendenceViewModel> GetAllAttendence();
        List<AttendenceViewModel> GetAttendenceForDate(DateTime date);
        List<AttendenceViewModel> GetAttendenceFörDeltagare(ObjectId deltagarId);
        void RemoveAttendenceItem(ObjectId Id);
        void UpdateAttendences(List<AttendenceViewModel> attendences);
        void UpdateAttendence(AttendenceViewModel attendence);
    }
}