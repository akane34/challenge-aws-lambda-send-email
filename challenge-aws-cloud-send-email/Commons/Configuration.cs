using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.cloud.cognito.trigger.Commons
{
    public static class Configuration
    {
        #region attributes
        private static string _emailSender;
        private static string _loginUrl;
        #endregion

        #region properties
        public static string EMAIL_SENDER
        {
            get 
            {
                if (_emailSender == null)
                {
                    _emailSender = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EMAIL_SENDER"))
                        ?
                        ""
                        :
                        Environment.GetEnvironmentVariable("EMAIL_SENDER");                
                }
                return _emailSender; 
            }
        }

        public static string LOGIN_URL
        {
            get
            {
                if (_loginUrl == null)
                    _loginUrl = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LOGIN_URL"))
                        ?
                        "https://"
                        :
                        Environment.GetEnvironmentVariable("LOGIN_URL");
                return _loginUrl;
            }
        }
        #endregion
    }
}
