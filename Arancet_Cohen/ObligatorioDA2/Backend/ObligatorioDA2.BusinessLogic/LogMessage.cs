using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic
{
    public  class LogMessage
    {
        public static readonly string LOGIN_OK = "Successful login.";
        public static readonly string LOGIN_USER_NOT_FOUND = "Login failed. User not found.";
        public static readonly string LOGIN_WRONG_PASSWORD = "Login failed. Wrong password.";
        public static readonly string LOGIN_DATAINACCESSIBLE = "Login failed. Data inaccessible.";
    }
}
