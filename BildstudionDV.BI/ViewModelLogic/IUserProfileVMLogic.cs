using BildstudionDV.BI.ViewModels;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IUserProfileVMLogic
    {
        string ChangePassword(UserProfileViewModel userModel);
        string CreateUserAccount(UserProfileViewModel viewModel);
        List<UserProfileViewModel> GetUserViewModels();
        string Login(UserProfileViewModel viewModel);
        string RemoveAccount(string username);
        void UpdateUser(UserProfileViewModel model);
    }
}