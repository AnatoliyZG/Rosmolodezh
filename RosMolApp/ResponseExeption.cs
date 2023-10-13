using RosMolExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RStatus = RosMolExtension.Response.Status;

namespace RosMolApp
{
    public class ResponseExeption : Exception
    {
        public string DisplayMessage => status switch
        {
            RStatus.Error => "Техническая ошибка, проверьте качество интернет соединения или потворите попытку позже.",
            RStatus.LoginFailed => "Проверьте правильность логина и пароля",
            RStatus.UserExists => "Пользователь уже существует",
            _ => errorCode,
        };

        public RStatus status;

        public string errorCode;

        public ResponseExeption(RStatus status) : base(status.ToString()) {
            errorCode = status.ToString();
            this.status = status;
        }

        public ResponseExeption(string errorCode) : base(errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
