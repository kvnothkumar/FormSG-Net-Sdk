using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormSG_Net_Sdk
{
    public class FormSGDencryptedDataModel
    {
        public string submissionId { get; set; }

        public FormSGDecryptedData[] data { get; set; }
    }

    public class FormSGDecryptedData
    {
        public string _id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string fieldType { get; set; }
        public bool isHeader { get; set; }
        public string[][] answerArray { get; set; }
        internal string fileUrl { get; set; }
        public byte[] fileContent { get; set; }
    }
}
