using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.BI.Models
{
    public class UserProfileModel
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
