using ServerAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.Models
{
    public class UserWithUserProfile
    {
        public User User { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}