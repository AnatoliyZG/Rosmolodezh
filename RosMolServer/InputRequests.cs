using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Text.Json.Serialization;
using System.Text.Json;
using SixLabors.ImageSharp;
using RosMolExtension;

namespace RosMolServer
{
    internal class InputRequests
    {
        private readonly DataBase dataBase;

        public InputRequests(DataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        public Response Input(NameValueCollection args, string content)
        {
            string? code = args["code"];

            return SwitchRequest(code).Invoke(args, content);
        }

        public RequestAction SwitchRequest(string? code) => code switch
        {
            "reg" => Register,
            "log" => Login,
            "data" => GetAnnonceData,
            _ => (a, b) => 1,
        };

        public static LoginResponse LoginData(LoginRequest loginRequest)
        {
            string userId = HashManager.GenerateMD5Hash(loginRequest.login);

            if (loginRequest is RegisterRequest registerRequest)
            {
                if (registerRequest.photo != null)
                {
                    Tools.SaveSquarePhoto(userId, registerRequest.photo, "avatars", 256);
                }
            }

            return new LoginResponse(userId);
        }


        public Response Register(NameValueCollection args, string content)
        {
            RegisterRequest? request = ParseRequest<RegisterRequest>(content);

            if (request == null)
            {
                return 1;
            }

            return LoginData(request);
        }


        public Response Login(NameValueCollection args, string content)
        {
            LoginRequest? request = ParseRequest<LoginRequest>(content);

            if (request == null)
            {
                return 1;
            }

            return LoginData(request);
        }

        public Response GetAnnonceData(NameValueCollection args, string content)
        {
            DataRequest? request = ParseRequest<DataRequest>(content);

            try
            {
                string? versionArg = args["version"];
                ulong? version = versionArg != null ? ulong.Parse(versionArg) : null;

                var data = dataBase.GetCachedContent<AnnounceData>(request?.key, version);

                return new DataResponse<AnnounceData>(data);
            }catch(ContentException ex) {

                return new DataResponse<AnnounceData>(ex.status);
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new DataResponse<AnnounceData>(Response.Status.Error);
            }
        }

        private static T? ParseRequest<T>(string? content) where T : class
        {
            if (content == null)
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { IncludeFields = true, MaxDepth=5 });
        }
    }
}
