using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOModels
{
    public class LoginRequest
    {
        public string UsernameOrMail { get; set; }
        public string Password { get; set; }
        public string SessionKey { get; set; }
    }
}
