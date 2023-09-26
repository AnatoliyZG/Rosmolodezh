using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Text.Json.Serialization;
using System.Text.Json;
using RosMolExtension;

namespace RosMolServer
{
    internal class InputRequests
    {
        public Response Input(NameValueCollection args)
        {
            string? code = args["code"];

            return switchRequest(code).Invoke(args);
        }

        private RequestAction switchRequest(string? code) => code switch
        {
            "reg" => register,
            "log" => login,
            _ => (a) => "Error request",
        };


        private Response register(NameValueCollection args)
        {
            RegisterRequest? request = parseRequest<RegisterRequest>(args["content"]);

            if (request == null)
            {
                return "Error request";
            }

            return new LoginResponse();
        }


        private Response login(NameValueCollection args)
        {
            LoginRequest? request = parseRequest<LoginRequest>(args["content"]);

            if (request == null)
            {
                return "Error request";
            }

            return new LoginResponse();
        }

        private T? parseRequest<T>(string? content) where T : class
        {
            if (content == null)
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { IncludeFields = true });
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
