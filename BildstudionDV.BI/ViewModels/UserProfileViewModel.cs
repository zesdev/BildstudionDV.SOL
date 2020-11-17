using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.ViewModels
{
    public class UserProfileViewModel
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string AssociatedGrupp { get; set; }
        public string NewPassword { get; set; }
    }
}
