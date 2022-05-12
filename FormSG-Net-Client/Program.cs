using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FormSG_Net_Sdk;

namespace FormSG_Net_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FormSGConstants.WebHookUri = "https://npdmavcrmaps04.azurewebsites.net/api/FormSG/Webhook";
            FormSGConstants.WebHookPublicKey = "3Tt8VduXsjjd4IrpdCd7BAkdZl/vUCstu9UvTX84FWw=";
            FormSGConstants.FormSGPrivateKey = "V3NQsJrBGbADsLXdU3tVJiIlBaStVRUE3klQtpNmvxU=";



            Crypto.DecryptWithAttachments(System.IO.File.ReadAllText(@"C:\Users\VK00685299\Desktop\FromSGData.txt"));
        }
    }
}
