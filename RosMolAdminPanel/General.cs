using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RosMolAdminPanel.General;

namespace RosMolAdminPanel
{
    internal static class General
    {
        private const string ServerUrl = "http://10.0.0.25:4447/connection/";

        private const string SecretKey = "root";


        public static async Task<T> GetServerResponse<T>(string code, string key) where T : class
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, ServerUrl);

                requestMessage.Headers.Add("code", code);
                requestMessage.Headers.Add("secret", SecretKey);
                requestMessage.Headers.Add("key", key);

                HttpClient client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(10),
                };

                HttpResponseMessage response = await client.SendAsync(requestMessage);

                string content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { MaxDepth = 5 });
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Подключение к серверу не установлено!", "Сервер", MessageBoxButtons.OK);
                return null;
            }
        }

        public static async void UploadDB(string key)
        {

            ServerResponse serverResponse = await GetServerResponse<ServerResponse>("serverDB", key);

            if (serverResponse == null)
                return;

            string message = "";

            switch (serverResponse.ResponseStatus)
            {
                case ServerResponse.Status.OK:
                    message = "Обновление прошло успешно.";
                    break;
                case ServerResponse.Status.NoneAuthorize:
                    message = "Ошибка авторизации.";
                    break;
                case ServerResponse.Status.Error:
                    message = "Оишбка сервера";
                    break;
                default:
                    message = "Данные сохранены, однако, сервер недоступен.";
                    break;
            }

            MessageBox.Show(message, serverResponse.ResponseStatus.ToString(), MessageBoxButtons.OK);
        }

        public static async Task<string[]> GetServerPhotos(string key)
        {
            return (await GetServerResponse<ServerArrayResponse<string>>("serverPhotos", key))?.content ?? null;
        }

        public static async Task<byte[]> GetServerPhoto(string key)
        {
            return (await GetServerResponse<ServerArrayResponse<byte>>("photo", key)).content;
        }

        [Serializable]
        public class ServerArrayResponse<ArrayT> : ServerResponse
        {
            public ArrayT[] content { get; set; }
        }

        [Serializable]
        public class ServerResponse
        {
            public int ErrorCode { get; set; }

            [JsonIgnore]
            public Status ResponseStatus => (Status)(ErrorCode);

            public enum Status : int
            {
                OK = 0,
                Error = 1,
                AlreadyUpdated = 2,
                NoneAuthorize = 3,
            }
        }
    }
}
