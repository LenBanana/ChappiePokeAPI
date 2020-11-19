using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModels.EntityModels
{
    public class RegisterCode
    {
        public RegisterCode()
        {
            Random rnd = new Random();
            Code = rnd.Next(1000000, 9999999);
        }

        public int RegisterCodeID { get; set; }
        public int Code { get; set; }
    }
}
