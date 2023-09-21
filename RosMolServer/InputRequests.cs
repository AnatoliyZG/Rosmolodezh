using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace RosMolServer
{
    internal class InputRequests
    {
        public string Input(NameValueCollection args)
        {
            string? code = args["code"];

            return switchRequest(code).Invoke(args);
        }

        private RequestAction switchRequest(string? code) => code switch
        {
            "reg" => register,
            "log" => login,
            _ => (a) => "Error code",
        };


        private string register(NameValueCollection args)
        {
            string? errorCode = checkFields(args, "phone", "login", "pass");

            if (errorCode != null)
            {
                return errorCode;
            }

            return "OK";
        }


        private string login(NameValueCollection args)
        {
            return "OK";
        }

        private string? checkFields(NameValueCollection args, params string[] keys)
        {
            foreach (string key in keys)
            {
                if (args[key] == null)
                {
                    return $"Error: {key} doesn't exist.";
                }
            }
            return null;
        }
    }
}
