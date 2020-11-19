using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models;
using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BildstudionDV.BI.ViewModelLogic
{
    public class DeltagareVMLogic : IDeltagareVMLogic
    {
        Deltagare deltagareDb;
        public DeltagareVMLogic(Deltagare _deltagareDb)
        {
            deltagareDb = _deltagareDb;
        }
        public void AddDeltagare(DeltagareViewModel viewModel)
        {
            int index = 0;
            try
            {
                var lastDeltagare = deltagareDb.GetAllDeltagarModels().LastOrDefault();
                index = lastDeltagare.IdAccess + 1;
            }
            catch
            {
            }
            var model = new DeltagareModel
            {
                IdAccess = index,
                DeltagarNamn = viewModel.DeltagarNamn,
                MatId = viewModel.MatId,
                Måndag = viewModel.Måndag.ToString(),
                Tisdag = viewModel.Tisdag.ToString(),
                Onsdag = viewModel.Onsdag.ToString(),
                Torsdag = viewModel.Torsdag.ToString(),
                Fredag = viewModel.Fredag.ToString(),
                IsActive = true
            };
            deltagareDb.AddDeltagare(model);
        }
        public void RemoveDeltagare(ObjectId Id)
        {
            deltagareDb.RemoveDeltagare(Id);
        }
        public List<DeltagareViewModel> GetAllDeltagare()
        {
            var returningList = new List<DeltagareViewModel>();
            var models = deltagareDb.GetAllDeltagarModels();
            foreach (var model in models)
            {
                var viewModel = new DeltagareViewModel
                {
                    IdAcesss = model.IdAccess,
                    DeltagarNamn = model.DeltagarNamn,
                    MatId = model.MatId,
                    Fredag = HelperConvertLogic.GetWorkDayFromString(model.Fredag),
                    Torsdag = HelperConvertLogic.GetWorkDayFromString(model.Torsdag),
                    Onsdag = HelperConvertLogic.GetWorkDayFromString(model.Onsdag),
                    Tisdag = HelperConvertLogic.GetWorkDayFromString(model.Tisdag),
                    Måndag = HelperConvertLogic.GetWorkDayFromString(model.Måndag),
                    Id = model.Id,
                    IsActive = model.IsActive
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
        public DeltagareViewModel GetDeltagare(ObjectId Id)
        {
            var model = deltagareDb.GetDeltagare(Id);
            var viewModel = new DeltagareViewModel
            {
                IdAcesss = model.IdAccess,
                DeltagarNamn = model.DeltagarNamn,
                MatId = model.MatId,
                Fredag = HelperConvertLogic.GetWorkDayFromString(model.Fredag),
                Torsdag = HelperConvertLogic.GetWorkDayFromString(model.Fredag),
                Onsdag = HelperConvertLogic.GetWorkDayFromString(model.Fredag),
                Tisdag = HelperConvertLogic.GetWorkDayFromString(model.Fredag),
                Måndag = HelperConvertLogic.GetWorkDayFromString(model.Fredag),
                Id = model.Id,
                IsActive = model.IsActive
            };
            return viewModel;
        }
        public void UpdateDeltagare(DeltagareViewModel viewModel)
        {
            var model = new DeltagareModel
            {
                IdAccess = viewModel.IdAcesss,
                DeltagarNamn = viewModel.DeltagarNamn,
                MatId = viewModel.MatId,
                Fredag = viewModel.Fredag.ToString(),
                Torsdag = viewModel.Torsdag.ToString(),
                Onsdag = viewModel.Onsdag.ToString(),
                Tisdag = viewModel.Tisdag.ToString(),
                Måndag = viewModel.Måndag.ToString(),
                Id = viewModel.Id,
                IsActive = viewModel.IsActive
            };
            deltagareDb.UpdateDeltagare(model);
        }
    }
}
