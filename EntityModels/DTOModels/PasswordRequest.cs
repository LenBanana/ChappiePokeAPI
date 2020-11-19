using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOModels
{
    public class PasswordRequest
    {
        public string Password { get; set; }
        public string SessionKey { get; set; }
    }
}
