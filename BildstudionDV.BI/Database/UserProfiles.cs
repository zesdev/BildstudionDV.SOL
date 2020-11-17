using BildstudionDV.BI.Context;
using BildstudionDV.BI.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BildstudionDV.BI.Database
{
    public class UserProfiles
    {
        BildStudionDVContext context;
        IMongoCollection<UserProfileModel> usersdb;
        public UserProfiles(BildStudionDVContext _context)
        {
            context = _context;
            usersdb = context.database.GetCollection<UserProfileModel>("users");
            if(usersdb.Find<UserProfileModel>(x => x.UserName == "admin").ToList().Count == 0)
            {
               
                var userModel = new UserProfileModel
                {
                    UserName = "admin",
                    Password = "Logon123#¤"
                };
                var newpassword = new PasswordHasher<UserProfileModel>().HashPassword(userModel, userModel.Password);
                userModel.Password = newpassword;
                usersdb.InsertOne(userModel);
                userModel = new UserProfileModel
                {
                    UserName = "piahag",
                    Password = "BildstudionLogon123"
                };
                newpassword = new PasswordHasher<UserProfileModel>().HashPassword(userModel, userModel.Password);
                userModel.Password = newpassword;
                usersdb.InsertOne(userModel);
            }
        }

        public List<UserProfileModel> GetAllUsers()
        {
            return usersdb.Find<UserProfileModel>(x => true).ToList();
        }
        public UserProfileModel GetUser(ObjectId Id)
        {
            return usersdb.Find<UserProfileModel>(x => x.Id == Id).First();
        }
        public void AddUser(UserProfileModel model)
        {
            var hashedpassword = new PasswordHasher<UserProfileModel>().HashPassword(model, model.Password);
            model.Password = hashedpassword; 
            usersdb.InsertOne(model);
        }
        public void RemoveUser(ObjectId Id)
        {
            usersdb.FindOneAndDelete<UserProfileModel>(x => x.Id == Id);
        }
        public string ChangePassword(UserProfileModel model)
        {
            try
            {
                var ogModel = GetAllUsers().First(x => x.UserName == model.UserName);
                var newpassword = new PasswordHasher<UserProfileModel>().HashPassword(ogModel, model.NewPassword);
                var verifyPassword = new PasswordHasher<UserProfileModel>().VerifyHashedPassword(ogModel, ogModel.Password, model.OldPassword);
                if (verifyPassword == PasswordVerificationResult.Success)
                {
                    UpdateProperty("Password", ogModel.Id, newpassword);
                    return "Lösenord bytt";
                }
                return "Lösenorden stämmer ej";
            }
            catch
            {
                return "Användaren existerar ej";
            }
        }
        public string Login(string username, string password)
        {
            try
            {
                var ogModel = GetAllUsers().First(x => x.UserName == username);
                var isPasswordsEquals = new PasswordHasher<UserProfileModel>().VerifyHashedPassword(ogModel, ogModel.Password, password);
                if (isPasswordsEquals == PasswordVerificationResult.Success)
                    return "Inloggad";
                else return "Ej Inloggad";
            }
            catch
            {
                return "Användaren existerar ej";
            }
        }
        private void UpdateProperty(string property, ObjectId IdOfItemBeingEdited, string newpropertyContent)
        {
            var filter = Builders<UserProfileModel>.Filter.Eq("Id", IdOfItemBeingEdited);
            var update = Builders<UserProfileModel>.Update.Set(property, newpropertyContent);
            usersdb.UpdateOne(filter, update);
        }
    }
}
