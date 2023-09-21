using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RosMolApp
{
    public static class General
    {
        private const string ServerUrl = "http://10.0.0.25:4447/connection/";

        public static string Name { get; set; }

        public static string Login { get; private set; }

        public static string Password { get; private set; }


        public static async Task<bool> Register(string name, string login, string password, string phone)
        {
            string response = await GetResponse("reg", ("login", login), ("pass", password), ("phone", phone));

            if(response != "OK")
            {
                throw new ResponseExeption(response);
            }

            Name = name;
            Login = login;
            Password = password;

            //TODO:

            return true;
        }

        private static async Task<string> GetResponse(string code, params (string key, string value)[] args)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, ServerUrl);

            requestMessage.Headers.Add("code", code);

            foreach (var arg in args)
            {
                requestMessage.Headers.Add(arg.key, arg.value);
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
