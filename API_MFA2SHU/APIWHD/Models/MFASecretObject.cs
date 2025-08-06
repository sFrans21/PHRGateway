namespace APIWHD.Models
{
    public class MFASecretObject
    {
        public string MFAAccount { get; set; }
        public string MFAIssuer { get; set; }
        public string MFASecret { get; set; }
        public string MFASecretTOTP { get; set; }
    }
}
