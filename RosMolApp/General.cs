using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosMolApp
{
    public static class General
    {
        public static string Name { get; set; }

        public static string Login { get; private set; }

        public static string Password { get; private set; }


        public static bool Register(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;

            //TODO:

            return true;
        }
    }
}
