﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BildStudionDV.Web.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }

        public IEnumerable<Users> GetUsers()
        {
            return new List<Users>() { new Users { Id = 101, UserName = "anet", Name = "Anet", EmailId = "anet@test.com", Password = "anet123" } };
        }
    }
}
