using BildstudionDV.BI.Database;
using BildstudionDV.BI.Models;
using NUnit.Framework;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace BildstudionDV.Test
{
    public class Tests
    {
        BildstudionDV.BI.Context.BildStudionDVContext context;
        UserProfiles usersDb;
        [SetUp]
        public void Setup()
        {
            var appSettingValFromStatic = ConfigurationManager.AppSettings["mySetting"];
            var username = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["username"].Value;
            var password = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["password"].Value;
            context = new BI.Context.BildStudionDVContext(username, password);
            usersDb = new UserProfiles(context);

        }

        [Test]
        public void a1TestAddUserProfile()
        {
            var userModel = new UserProfileModel
            {
                UserName= "erik",
                Password= "Logon123"
            };
            usersDb.AddUser(userModel);
            Assert.AreEqual(2, usersDb.GetAllUsers().Count);
        }
        [Test]
        public void a2TestChangePassword()
        {
            Assert.AreEqual("Inloggad", usersDb.Login("erik", "Logon123"));
            var userModel = new UserProfileModel
            {
                UserName = "erik",
                Password = "Logon123",
                OldPassword = "Logon123",
                NewPassword = "Logon234"
            };
            Assert.AreEqual("Lösenord bytt", usersDb.ChangePassword(userModel));
            Assert.AreEqual("Inloggad", usersDb.Login("erik", "Logon234"));
            Assert.AreEqual("Ej Inloggad", usersDb.Login("erik", "Logon123"));
            Assert.AreEqual("Användaren existerar ej", usersDb.Login("adolf", "bragrejerattyskland"));
        }
        [Test]
        public void z1RemoveUserProfile()
        {
            var user = usersDb.GetAllUsers().First(x => x.UserName == "erik");
            usersDb.RemoveUser(user.Id);
            Assert.AreEqual(1, usersDb.GetAllUsers().Count);
        }
    }
}