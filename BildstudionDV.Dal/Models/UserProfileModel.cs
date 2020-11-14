using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BildstudionDV.Dal.Models
{

    public class UserProfileModel
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
