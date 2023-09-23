using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using RosMolExtension;

namespace RosMolApp
{
    public static class General
    {
        private const string ServerUrl = "http://10.0.0.25:4447/connection/";

        public static string Name { get; set; }

        public static string Login { get; private set; }

        public static string Password { get; private set; }


        public static async Task<bool> Register(RegisterRequest registerRequest)
        {
            LoginResponse response = await GetResponse<LoginResponse, RegisterRequest>("reg", registerRequest);

            Console.WriteLine($"Response status: {response.ResponseStatus}");

            if (response.ResponseStatus != Response.Status.OK)
            {
                throw new ResponseExeption(response.ErrorCode);
            }

            //TODO:

            return true;
        }

        public static async Task<bool> LoginAccount(string login, string pass)
        {
            // string response = await GetResponse("log", ("login", login), ("pass", pass));

            return true;
        }

        private static async Task<TResponse> GetResponse<TResponse, TRequest>(string code, TRequest request) where TResponse : Response where TRequest : Request
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, ServerUrl);
            requestMessage.Headers.Add("code", code);

            requestMessage.Headers.Add("content", JsonSerializer.Serialize(request, typeof(TRequest), new JsonSerializerOptions() { IncludeFields=true} ));

            HttpClient client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10),
            };

            try
            {
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                return JsonSerializer.Deserialize<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { IncludeFields = true });
            }
            catch (TaskCanceledException)
            {
                return (TResponse)"Техническая ошибка, проверьте качество интернет соединения или потворите попытку позже.";
            }
        }
    }
}
