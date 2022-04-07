using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormSG_Net_Sdk
{
    public class WebhookAuthenticateException : Exception
    {
        public WebhookAuthenticateException(String message) : base(message) { } 
    }

    public class InvalidPayLoadException : Exception
    {
        public InvalidPayLoadException(String message) : base(message) { }
    }
}
