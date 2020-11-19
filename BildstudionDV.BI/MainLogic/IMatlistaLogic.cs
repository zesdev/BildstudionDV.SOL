using BildstudionDV.BI.ViewModels;
using System.Collections.Generic;

namespace BildstudionDV.BI.MainLogic
{
    public interface IMatlistaLogic
    {
        List<MatListaMonthViewModel> GetAttendenceForMonth(int month, int year);
        void UpdatePris(int newPris);
    }
}