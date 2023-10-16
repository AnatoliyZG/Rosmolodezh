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
        private const string ServerUrl = "http://188.225.34.103:4447/connection/";

        public static string Photo { get; set; }

        public static string UserId { get; set; }

        private static JsonSerializerOptions defaultOptions = new JsonSerializerOptions()
        {
            MaxDepth = 5,
            IncludeFields = true,
        };

        private const ushort secretKey = 0x1288;


        public static async Task<bool> Register(RegisterRequest registerRequest)
        {
            LoginResponse response = await GetResponse<LoginResponse, RegisterRequest>("reg", registerRequest);

            Console.WriteLine($"Registration response status: {response.ResponseStatus}");

            if (response.ResponseStatus != Response.Status.OK)
            {
                throw new ResponseExeption(response.ResponseStatus);
            }

            UserId = response.userId;
            //TODO:

            return true;
        }

        public static async Task<bool> LoginAccount(LoginRequest loginRequest)
        {
            LoginResponse response = await GetResponse<LoginResponse, LoginRequest>("log", loginRequest);

            if (response.ErrorCode != 0)
            {
                throw new ResponseExeption(response.ResponseStatus);
            }

            UserId = response.userId;

            return true;
        }

        internal static void SaveAccountCache(string login, string password)
        {
            File.WriteAllLines(CachePath("acc"), new string[2]{
                EncodeDecrypt(login, secretKey),
                EncodeDecrypt(password, secretKey) });
        }

        internal static (string login, string password)? LoadAccountCache()
        {
            if (!File.Exists(CachePath("acc")))
                return null;

            try
            {
                string[] strs = File.ReadAllLines(CachePath("acc"));

                return (EncodeDecrypt(strs[0], secretKey), EncodeDecrypt(strs[1], secretKey));
            }
            catch
            {
                DeleteAccounteCache();
                return null;
            }
        }

        internal static void DeleteAccounteCache()
        {
            File.Delete(CachePath("acc"));
        }

        private static string EncodeDecrypt(string str, ushort secretKey)
        {
            if (str == null)
                return string.Empty;

            var ch = str.ToArray();
            string newStr = "";
            foreach (var c in ch)
                newStr += TopSecret(c, secretKey);
            return newStr;
        }

        private static char TopSecret(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey);
            return character;
        }

        public static async Task<ImageSource> RequestImage(PhotoRequest request)
        {
            string? cacheVersion = null;

            string path = $"{request.key}_{request.name}.jpg";

            if (File.Exists(CachePath(path)))
            {
                cacheVersion = CacheVersion(path);
            }
            path = (CachePath(path));

            PhotoResponse response = await GetResponse<PhotoResponse, PhotoRequest>("photo", request, false, cacheVersion);

            if (response.ResponseStatus != Response.Status.OK)
            {
                if (response.ResponseStatus == Response.Status.AlreadyUpdated)
                {
                    if (File.Exists(path))
                    {
                        return ImageSource.FromStream(() => new StreamReader(path).BaseStream);
                    }
                }

                return null;
            }
            File.WriteAllBytes(path, response.content);

            return ImageSource.FromStream(() => new MemoryStream(response.content));
        }

        public static async Task<Data[]> RequestData<Data>(DataRequest dataRequest, bool cache = true) where Data : ReadableData
        {
            string cacheVersion = cache ? CacheVersion(dataRequest.key) : null;

            DataResponse<Data> response = await GetResponse<DataResponse<Data>, DataRequest>("data", dataRequest, true, cacheVersion);

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

                throw new ResponseExeption(response.ResponseStatus);
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

        private static async Task<TResponse> GetResponse<TResponse, TRequest>(string code, TRequest request, bool userId = false, string version = null) where TResponse : Response where TRequest : Request
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, ServerUrl);
                requestMessage.Headers.Add("code", code);

                if (userId)
                {
                    requestMessage.Headers.Add("userId", UserId);
                }

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
                throw new ResponseExeption(Response.Status.Error);
            }
            catch (Exception ex)
            {
                throw new ResponseExeption(ex.Message);
            }
        }
    }
}
