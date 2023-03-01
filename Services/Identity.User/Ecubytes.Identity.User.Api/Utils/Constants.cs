using System;
using System.Linq;

namespace Ecubytes.Identity.User.Api.MessagesCodes
{
    public enum LoginCodes
    {
        Ok = 2000,        
        InvalidLogonNameOrPassword = 2001,
        BlockLogin = 2002
    }
}
