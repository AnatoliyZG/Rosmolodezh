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
        public string UserId;

        public string? key;

        public DataRequest(string UserId, string? key)
        {
            this.UserId = UserId;
            this.key = key;
        }
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
        public int? ErrorCode;


        [JsonIgnore]
        public Status ResponseStatus => (Status)(ErrorCode ?? 0);

        public enum Status : int
        {
            OK = 0,
            Error = 1,
            AlreadyUpdated = 2,
        }

        public abstract override string ToString();

        public static implicit operator Response(int errorCode) => new ErrorResponse(errorCode);

        public Response(int? errorCode)
        {
            ErrorCode = errorCode;
        }
    }

    [Serializable]
    public class ErrorResponse : Response
    {
        public ErrorResponse(int? ErrorCode) : base(ErrorCode) {}

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
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
            return JsonSerializer.Serialize<LoginResponse>(this);
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

            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                IncludeFields = true,
                MaxDepth=5,
            });
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
                    name = value.GetString(0).Trim();
                    summary = value.GetString(1).Trim();
                    description = value.GetString(2).Trim();
                    photoName = value.GetString(3).Trim();
                }
            }
        }

        public string? name;

        public string? summary;

        public string? description;

        [JsonIgnore]
        public string? photoName;

        public AnnounceData(string? name, string? summary, string? description)
        {
            this.name = name;
            this.summary = summary;
            this.description = description;
        }

        public AnnounceData() { }
    }
}