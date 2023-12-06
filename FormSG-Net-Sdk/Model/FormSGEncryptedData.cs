using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormSG_Net_Sdk
{
    public class FormSGEncryptedDataModel
    {
        public FormSGEncryptedData data { get; set; }
    }

    public class FormSGEncryptedData
    {
        public string formId { get; set; }
        public string submissionId { get; set; }
        public string encryptedContent { get; set; }
        public decimal version { get; set; }
        public DateTime created { get; set; }
        public Dictionary<String, String> attachmentDownloadUrls { get; set; }
    }
}
