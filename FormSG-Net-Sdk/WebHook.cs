using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chaos.NaCl;


namespace FormSG_Net_Sdk
{
    public static class WebHook
    {
        private static bool VerifyEpoch(ulong epoch, int expiryInMinutes)
        {
            DateTime signatureTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            signatureTime = signatureTime.AddSeconds(epoch).AddMinutes(expiryInMinutes);

            return DateTime.Now > signatureTime;
        }

        public static bool Authenticate(String strSignature, int expiryInMinutes = 0)
        {
            if (String.IsNullOrEmpty(FormSGConstants.WebHookUri))
                throw new WebhookAuthenticateException($"Please initialize the Webhook URI");

            if (String.IsNullOrEmpty(FormSGConstants.WebHookPublicKey))
                throw new WebhookAuthenticateException($"Please initialize the Webhook Public Key");

            var parameters = ParseSignature(strSignature);

            if (parameters.Keys.Count != 4)
                throw new WebhookAuthenticateException($"Invalid Signature format {strSignature}");

            var baseString = $"{FormSGConstants.WebHookUri}.{parameters["s"]}.{parameters["f"]}.{parameters["t"]}";

            var message = Encoding.UTF8.GetBytes(baseString);
            var signature = Convert.FromBase64String(parameters["v1"]);
            var webhookKey = Convert.FromBase64String(FormSGConstants.WebHookPublicKey);

            bool isValid = Ed25519.Verify(signature, message, webhookKey);

            if (!isValid)
                throw new WebhookAuthenticateException($"Signature could not be verified for uri={FormSGConstants.WebHookUri} submissionId={parameters["s"]} formId={parameters["f"]} epoch={parameters["t"]} signature=${parameters["v1"]}");

            if (expiryInMinutes > 0)
            {
                bool isExpired = VerifyEpoch(Convert.ToUInt64(parameters["t"]), expiryInMinutes);

                if (isExpired)
                    throw new WebhookAuthenticateException($"Signature is not recent for uri={FormSGConstants.WebHookUri} submissionId={parameters["s"]} formId={parameters["f"]} epoch={parameters["t"]} signature=${parameters["v1"]}");
            }

            return true;
        }

        private static Dictionary<String, String> ParseSignature(string value)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();

            try
            {
                foreach (var item in value.Split(','))
                {
                    var kv = System.Text.RegularExpressions.Regex.Split(item, "=(.*)");
                    result.Add(kv[0], kv[1]);
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}
