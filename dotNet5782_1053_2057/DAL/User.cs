﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalXml
{
    namespace DO
    {
        public struct User 
        {
            public User(string _username, string _password, int _id = -1)
            {
                Username = _username;
                Password = _password;
                Id = _id;
            }
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
       


    }
}
