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
using System.IO;

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
            "photo" => Photo,
            "serverDB" => ServerResponse,
            "serverPhotoPull" => PullPhoto,
            "serverPhotos" => ServerPhotosResponse,
            _ => (a, b) => 1,
        };

        public Response LoginData(LoginRequest loginRequest)
        {
            try
            {
                string userId = HashManager.GenerateMD5Hash(loginRequest.login);

                if (loginRequest is RegisterRequest registerRequest)
                {
                    if (dataBase.GetUser(loginRequest.login) != null)
                    {
                        return 5;
                    }

                    dataBase.AddUser(loginRequest);

                    if (registerRequest.photo != null)
                    {
                        Tools.SaveSquarePhoto(userId, registerRequest.photo, "Users", 256);
                    }
                }
                else
                {
                    var user = dataBase.GetUser(loginRequest.login);

                    if(user == null || user.Password != loginRequest.password)
                    {
                        return 4;
                    }
                }

                return new LoginResponse(userId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new ErrorResponse(Response.Status.Error);
            }
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

        public Response Photo(NameValueCollection args, string content)
        {
            try
            {
                PhotoRequest? request = ParseRequest<PhotoRequest>(content);

                if (request == null)
                {
                    return new PhotoResponse(File.ReadAllBytes($"{args["key"]}"));
                }
                else
                {
                    string? version = args["version"];

                    if (version != null)
                    {
                        ulong cur = ulong.Parse(File.GetLastWriteTimeUtc($"{request.key}/{request.name}.jpg").ToString("yyyyMMddHHmmss"));
                        if (cur < ulong.Parse(version))
                        {
                            return 2;
                        }
                    }

                    return new PhotoResponse(File.ReadAllBytes($"{request.key}/{request.name}.jpg"));
                }
            }
            catch
            {
                return 2;
            }
        }

        public Response? PullPhoto(NameValueCollection args, string content)
        {
            string? secret = args["secret"];

            if (secret == null || secret != dataBase.secretKey)
            {
                return 3;
            }
            var photo = ParseRequest<byte[]>(content);

            File.WriteAllBytes(args["key"]!, photo!);
            //File.SetLastWriteTimeUtc(args["key"]!, DateTime.UtcNow);
            return 0;
        }

        public Response GetAnnonceData(NameValueCollection args, string content)
        {
            DataRequest? request = ParseRequest<DataRequest>(content);

            string? key = request?.key;
            switch (key)
            {
                case "Events":
                    return GetData<EventData>(args["version"], key);
                case "News":
                    return GetData<NewsData>(args["version"], key);
                default:
                    return GetData<AnnounceData>(args["version"], key);

            }
        }

        private Response GetData<T>(string? ver, string? key) where T : ReadableData, new()
        {
            try
            {
                ulong? version = ver != null ? ulong.Parse(ver) : null;

                var data = dataBase.GetCachedContent<T>(key, version);

                return new DataResponse<T>(data);
            }
            catch (ContentException ex)
            {

                return new DataResponse<T>(ex.status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new DataResponse<T>(Response.Status.Error);
            }
        }

        public ErrorResponse ServerResponse(NameValueCollection args, string content)
        {
            string? secret = args["secret"];

            if (secret == null || secret != dataBase.secretKey)
            {
                return new ErrorResponse(Response.Status.NoneAuthorize);
            }

            try
            {
                string key = args["key"]!;

                if (key == "Events")
                {
                    dataBase.ReloadDB<EventData>(key);
                }
                else if (key == "News")
                {
                    dataBase.ReloadDB<NewsData>(key);
                }
                else
                {
                    dataBase.ReloadDB<AnnounceData>(key);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new ErrorResponse(Response.Status.Error);
            }

            return new ErrorResponse(Response.Status.OK);
        }

        public SimpleResponse<string[]> ServerPhotosResponse(NameValueCollection args, string content)
        {
            string? secret = args["secret"];

            if (secret == null || secret != dataBase.secretKey)
            {
                return new(3);
            }

            try
            {
                return Tools.GetPhotos(args["key"]!);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new(1);
            }
        }

        private static T? ParseRequest<T>(string? content) where T : class
        {
            if (content == null || content.Length == 0)
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { IncludeFields = true, MaxDepth=5 });
        }
    }
}
