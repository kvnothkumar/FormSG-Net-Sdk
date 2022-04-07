using Chaos.NaCl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FormSG_Net_Sdk
{
    public static class Crypto
    {
        private static Byte[] DecryptContent(String submissionPublicKey, String strNonce, String encryptedContent)
        {
            if (String.IsNullOrEmpty(FormSGConstants.FormSGPrivateKey))
                throw new WebhookAuthenticateException($"Please initialize the Form SG Private Key");

            var publicKey = Convert.FromBase64String(submissionPublicKey);
            var privateKey = Convert.FromBase64String(FormSGConstants.FormSGPrivateKey);

            var nonce = Convert.FromBase64String(strNonce);
            var cipherText = Convert.FromBase64String(encryptedContent);

            var key = MontgomeryCurve25519.KeyExchange(publicKey, privateKey);

            return XSalsa20Poly1305.TryDecrypt(cipherText, key, nonce);
        }

        public static List<FormSGDecryptedData> Decrypt(String payLoad)
        {
            var encData = JsonConvert.DeserializeObject<FormSGEncryptedDataModel>(payLoad);

            if (encData == null)
                throw new InvalidPayLoadException($"Payload is not in the Valid Format");

            var values1 = encData.data.encryptedContent.Split(';');
            string submissionPublicKey = values1[0], nonceEncrypted = values1[1];

            var values2 = nonceEncrypted.Split(':');
            string nonce = values2[0], encryptedContent = values2[1];

            var result = DecryptContent(submissionPublicKey, nonce, encryptedContent);

            var decryptedData = JsonConvert.DeserializeObject<List<FormSGDecryptedData>>(Encoding.UTF8.GetString(result));

            foreach (var item in decryptedData)
                if (item.fieldType == "attachment" && !String.IsNullOrEmpty(item.answer))
                    item.fileUrl = encData.data.attachmentDownloadUrls.Where(i => i.Key == item._id).Select(i => i.Value).FirstOrDefault();

            return decryptedData;
        }

        public static List<FormSGDecryptedData> DecryptWithAttachments(String payLoad)
        {
            var decryptedData = Decrypt(payLoad);

            using (WebClient wc = new WebClient())
            {
                foreach (var item in decryptedData)
                {
                    if (!String.IsNullOrEmpty(item.fileUrl))
                    {
                        var json = wc.DownloadString(item.fileUrl);
                        var fileData = JsonConvert.DeserializeObject<FormSGEncryptedFileModel>(json);

                        var nonce = fileData.encryptedFile.nonce;
                        var submissionPublicKey = fileData.encryptedFile.submissionPublicKey;

                        item.fileContent = DecryptContent(submissionPublicKey, nonce, fileData.encryptedFile.binary);
                    }
                }
            }

            return decryptedData;
        }
    }
}
