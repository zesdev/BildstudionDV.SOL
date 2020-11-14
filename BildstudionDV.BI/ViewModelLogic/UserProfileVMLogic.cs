using BildstudionDV.BI.Context;
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
    public class UserProfileVMLogic
    {
        UserProfiles usersDb;
        public UserProfileVMLogic(UserProfiles _usersDb)
        {
            usersDb = _usersDb;
        }
        public string CreateUserAccount(UserProfileViewModel viewModel)
        {
            if (viewModel.UserName == "")
                return "Användarnamn saknas";
            if (viewModel.Password == "")
                return "Lösenord saknas";
            if (usersDb.GetAllUsers().Any(x => x.UserName.ToLower() == viewModel.UserName.ToLower()))
                return "Användarnamnet uptaget, försök med något annat";
            var userModel = new UserProfileModel { UserName = viewModel.UserName, Password = viewModel.Password };
            usersDb.AddUser(userModel);
            return "Success";
        }
        public string RemoveAccount(string username)
        {
            var user = usersDb.GetAllUsers().First(x => x.UserName == username);
            if (user == null)
                return "Användarn existerar ej, går inte att ta bort";
            usersDb.RemoveUser(user.Id);
            return username + " är nu borttagen";
        }
        public string Login(UserProfileViewModel viewModel)
        {
            return usersDb.Login(viewModel.UserName, viewModel.Password);
        }
        public List<UserProfileViewModel> GetUserViewModels()
        {
            var returningList = new List<UserProfileViewModel>();
            var modelsRaw = usersDb.GetAllUsers();
            foreach (var model in modelsRaw)
            {
                var viewModel = new UserProfileViewModel
                {
                    Id = model.Id,
                    UserName = model.UserName
                };
                returningList.Add(viewModel);
            }
            return returningList;
        }
    }
}
