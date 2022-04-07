using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormSG_Net_Sdk
{
    public class FormSGEncryptedFileModel
    {
        public FormSGEncryptedFile encryptedFile { get; set; }
    }

    public class FormSGEncryptedFile
    {
        public string submissionPublicKey { get; set; }
        public string nonce { get; set; }
        public string binary { get; set; }
    }
}
