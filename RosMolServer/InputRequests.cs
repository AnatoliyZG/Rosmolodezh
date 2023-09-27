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
        public Response Input(NameValueCollection args, string content)
        {
            string? code = args["code"];

            return switchRequest(code).Invoke(args, content);
        }

        private RequestAction switchRequest(string? code) => code switch
        {
            "reg" => register,
            "log" => login,
            _ => (a, b) => "Error request",
        };


        private Response register(NameValueCollection args, string content)
        {
            RegisterRequest? request = parseRequest<RegisterRequest>(content);

            if (request == null)
            {
                return "Error request";
            }

            if(request.photo != null)
            {
                savePhoto(request.login, request.photo);
            }

            return "Test error";
        }


        private Response login(NameValueCollection args, string content)
        {
            LoginRequest? request = parseRequest<LoginRequest>(content);

            if (request == null)
            {
                return "Error request";
            }

            return new LoginResponse();
        }

        private void savePhoto(string login, byte[] photo)
        {
            if(!Directory.Exists("images")) {
                Directory.CreateDirectory("images");
            }
            Image img = Image.Load(photo);

            int resolution = Math.Min(img.Width, img.Height);

            img.Mutate(i=>
            {
                i.Resize(new ResizeOptions()
                {
                    Size = new Size(resolution, resolution),
                    Mode = ResizeMode.Crop,
                });
                if (resolution > 256)
                {
                    i.Resize(new ResizeOptions()
                    {
                        Size = new Size(256, 256),
                        Mode = ResizeMode.Stretch,
                    });
                }
            });
            img.SaveAsJpeg($"images/{login}.jpeg");

            img.Dispose();

        }

        private T? parseRequest<T>(string? content) where T : class
        {
            if (content == null)
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { IncludeFields = true, MaxDepth=5 });
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
