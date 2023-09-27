using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RosMolExtension
{
    [Serializable]
    public class LoginRequest : Request
    {
        public string login;
        public string password;

        public LoginRequest(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }

    [Serializable]
    public class RegisterRequest : LoginRequest
    {
        public string? name;

        public string? phone;

        public string? city;

        public string? vkLink;

        public string? direction;

        public byte[]? photo;

        public DateTime bornDate;

        public RegisterRequest(string login, string password) : base(login, password) { }
    }

    [Serializable]
    public class Photo
    {
        public string format;
        public byte[] photo;

        [JsonConstructor]
        public Photo(string format, byte[] photo)
        {
            this.format = format;
            this.photo = photo;
        }

        public Photo(string path)
        {
            this.format = path.Split('.')[^1];
            photo = File.ReadAllBytes(path);
        }
    }

    public abstract class Request
    {
    }

    public abstract class Response
    {
        [JsonInclude]
        public string? ErrorCode;

        [JsonIgnore]
        public Status ResponseStatus => ErrorCode == null ? Status.OK : Status.Error;

        public enum Status
        {
            OK,
            Error,
        }

        public abstract override string ToString();

        public static implicit operator Response(string errorCode) => new SimpleResponse(errorCode);
    }

    [Serializable]
    public class SimpleResponse : Response
    {
        public SimpleResponse(string? ErrorCode)
        {
            this.ErrorCode = ErrorCode;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    [Serializable]
    public class LoginResponse : Response
    {
        public override string ToString()
        {
            return JsonSerializer.Serialize<LoginResponse>(this);
        }
    }
}