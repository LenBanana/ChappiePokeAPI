using Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOResponseModels
{
    public class RegisterResponse
    {
        public RegisterError RegisterError { get; set; }
        public string SessionKey { get; set; }
        public string PasswordSalt { get; set; }

        public RegisterResponse()
        {
            
        }
    }
}
