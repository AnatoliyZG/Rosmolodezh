using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Data.Common;
using System.Data;

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
    public class DataRequest : Request
    {
        public string? key;

        public DataRequest(string? key)
        {
            this.key = key;
        }
    }

    [Serializable]
    public class PhotoRequest : Request
    {
        public string? key;

        public string? name;

        public PhotoRequest(string? key, string? name)
        {
            this.key= key;
            this.name = name;
        }
    }

    public abstract class Request
    {
    }

    public abstract class Response
    {
        [JsonInclude]
        public int? ErrorCode;


        [JsonIgnore]
        public Status ResponseStatus => (Status)(ErrorCode ?? 0);

        public enum Status : int
        {
            OK = 0,
            Error = 1,
            AlreadyUpdated = 2,
            NoneAuthorize = 3,
        }

        public abstract override string ToString();

        public static implicit operator Response(int errorCode) => new ErrorResponse(errorCode);

        public Response(int? errorCode)
        {
            ErrorCode = errorCode;
        }
    }

    public class SimpleResponse<T> : Response
    {
        public T content;

        [JsonConstructor]
        public SimpleResponse(T content) : base (0)
        {
            this.content = content;
        }

        public SimpleResponse(int? errorCode) : base(errorCode)
        {

        }

        public override string ToString()
        {
            return Serializer.Serialize(this);
        }
    }

    [Serializable]
    public class ErrorResponse : Response
    {
        [JsonConstructor]
        public ErrorResponse(int? ErrorCode) : base(ErrorCode) {}

        public ErrorResponse(Status errorStatus) : base((int)errorStatus) { }

        public override string ToString()
        {
            return Serializer.Serialize(this);
        }
    }

    [Serializable]
    public class LoginResponse : Response
    {
        public string userId;

        public LoginResponse(string userId) : base(0)
        {
            this.userId = userId;
        }

        public override string ToString()
        {
            return Serializer.Serialize(this);
        }
    }

    [Serializable]
    public class PhotoResponse : Response
    {
        public byte[]? content;

        public PhotoResponse(byte[]? content) : base(0)
        {
            this.content = content;
        }

        public override string ToString()
        {
            return Serializer.Serialize(this);
        }
    }

    [Serializable]
    public class DataResponse<Data> : Response where Data : ReadableData
    {
        public Data[]? Content;

        [JsonConstructor]
        public DataResponse(Data[]? content) : base(0)
        {
            Content = content;
        }

        public DataResponse(Status status) : base((int)status) { }

        public override string ToString()
        {
            if(Content == null) {

                if (ErrorCode == 0)
                {
                    ErrorCode = 1;
                }
            }

            return Serializer.Serialize(this);
        }
    }

    public abstract class ReadableData
    {
        [JsonIgnore]
        public abstract DbDataReader Reader { set; }

        public ReadableData() { }
    }

    [Serializable]
    public class AnnounceData : ReadableData
    {
        public override DbDataReader Reader { 
            set {
                if(value != null)
                {
                    name = value.IsDBNull(1) ? null : value.GetString(1).Trim();
                    summary = value.IsDBNull(2) ? null : value.GetString(2).Trim();
                    description = value.IsDBNull(3) ? null : value.GetString(3).Trim();
                }
            }
        }

        public string? name;

        public string? summary;

        public string? description;

        public AnnounceData(string? name, string? summary, string? description)
        {
            this.name = name;
            this.summary = summary;
            this.description = description;
        }

        public AnnounceData() { }
    }

    public static class Serializer
    {
        public static string Serialize<Data>(Data data) => JsonSerializer.Serialize(data, new JsonSerializerOptions() { IncludeFields = true, MaxDepth=5, });
    }
}