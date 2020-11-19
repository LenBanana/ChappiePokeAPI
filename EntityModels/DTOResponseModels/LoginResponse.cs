using EntityModels.EntityModels;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOResponseModels
{
    public class LoginResponse
    {
        public LoginError LoginError { get; set; }
        public string SessionKey { get; set; }

        public LoginResponse(User user)
        {
            LoginError = user == null ? LoginError.UsernameNotExist : (user.UserPrivileges > 0 ? LoginError.NoError : LoginError.NotVerified);
            SessionKey = user == null ? "" : user.SessionKey;
        }
    }
}
