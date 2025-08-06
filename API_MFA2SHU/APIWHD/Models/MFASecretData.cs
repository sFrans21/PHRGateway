namespace APIWHD.Models
{
    public class MFASecretData
    {
        public string MFASecret { get; set; }
        public string MFASecretTOTP { get; set; }
        public string MFAAccount { get; set; }
        public string MFAIssuer { get; set; }
        public bool IsFromGet { get; set; }
    }
}
