using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOModels
{
    public class VerificationRequest
    {
        public string SessionKey { get; set; }
        public int VerificationCode { get; set; }
    }
}
