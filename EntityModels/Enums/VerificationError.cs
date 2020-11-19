using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Enums
{
    public enum VerificationError
    {
        NoError,
        WrongToken,
        WrongSessionKey,
        OtherError
    }
}
