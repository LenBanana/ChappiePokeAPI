using EntityModels.EntityModels;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOModels
{
    public class UserPrivilegeResponse
    {
        public UserPrivilegeResponse(User user)
        {
            if (user == null)
                UserPrivileges = 0;
            else
                UserPrivileges = user.UserPrivileges;
        }
        public UserPrivileges UserPrivileges { get; set; }
    }
}
