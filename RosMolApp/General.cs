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

        private static JsonSerializerOptions defaultOptions = new JsonSerializerOptions()
        {
            MaxDepth = 5,
            IncludeFields = true,
        };


        public static async Task<bool> Register(RegisterRequest registerRequest)
        {
            LoginResponse response = await GetResponse<LoginResponse, RegisterRequest>("reg", registerRequest);

            Console.WriteLine($"Registration response status: {response.ResponseStatus}");

            if (response.ResponseStatus != Response.Status.OK)
            {
                throw new ResponseExeption(response.ResponseStatus.ToString());
            }

            //TODO:

            return true;
        }

        public static async Task<bool> LoginAccount(LoginRequest loginRequest)
        {
            LoginResponse response = await GetResponse<LoginResponse, LoginRequest>("reg", loginRequest);

            // string response = await GetResponse("log", ("login", login), ("pass", pass));

            return true;
        }

        public static async Task<Data[]> RequestData<Data>(DataRequest dataRequest, bool cache = true) where Data : ReadableData
        {
            string cacheVersion = cache ? CacheVersion(dataRequest.key) : null;

            DataResponse<Data> response = await GetResponse<DataResponse<Data>, DataRequest>("data", dataRequest, cacheVersion);

            Console.WriteLine($"Request data response status: {response.ResponseStatus}");

            if (response.ResponseStatus != Response.Status.OK)
            {
                if (TryLoadFromCache<Data>(dataRequest.key, out var data))
                {
                    return data;
                }
                else if (response.ResponseStatus == Response.Status.AlreadyUpdated)
                {
                    DeleteCache(dataRequest.key);
                    return await RequestData<Data>(dataRequest, cache);
                }

                throw new ResponseExeption(response.ResponseStatus.ToString());
            }

            if (cache)
            {
                File.WriteAllText(CachePath(dataRequest.key), JsonSerializer.Serialize(response.Content, defaultOptions));
            }

            return response.Content;
        }

        public static void DeleteCache(string key) => File.Delete(CachePath(key));


        public static string CacheVersion(string key)
        {
            string path = CachePath(key);

            if (File.Exists(path))
            {
                return File.GetLastWriteTimeUtc(path).ToString("yyyyMMddHHmmss");
            }

            return null;
        }

        public static bool TryLoadFromCache<Data>(string key, out Data[] data) where Data : ReadableData
        {
            string path = CachePath(key);

            if (File.Exists(path))
            {
                data = JsonSerializer.Deserialize<Data[]>(File.ReadAllText(path), defaultOptions);

                return true;
            }

            data = null;

            return false;
        }

        private static string CachePath(string key) => $"{FileSystem.Current.CacheDirectory}/{key}";

        private static async Task<TResponse> GetResponse<TResponse, TRequest>(string code, TRequest request, string version = null) where TResponse : Response where TRequest : Request
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, ServerUrl);
                requestMessage.Headers.Add("code", code);

                if (version != null)
                {
                    requestMessage.Headers.Add("version", version);
                }

                requestMessage.Content = JsonContent.Create<TRequest>(request, options: defaultOptions);

                HttpClient client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(10),
                };

                HttpResponseMessage response = await client.SendAsync(requestMessage);

                return JsonSerializer.Deserialize<TResponse>(await response.Content.ReadAsStringAsync(), defaultOptions);
            }
            catch (TaskCanceledException)
            {
                throw new ResponseExeption("Техническая ошибка, проверьте качество интернет соединения или потворите попытку позже.");
            }
            catch (Exception ex)
            {
                throw new ResponseExeption(ex.Message);
            }
        }
    }
}
