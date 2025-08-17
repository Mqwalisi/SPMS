using System;
using System.Collections.Generic;
using System.Linq;
using SPMS;

namespace SPMS
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // In production, use hashed passwords!
        public string Role { get; set; } // "User" or "Admin"
    }
}