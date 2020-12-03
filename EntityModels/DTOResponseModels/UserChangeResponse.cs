using Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOResponseModels
{
    public class UserChangeResponse
    {
        public UserChangeResponse(GenericError error)
        {
            Error = error;
        }
        public GenericError Error { get; set; }
    }
}
