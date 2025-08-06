namespace APIWHD.Models
{
    public class PushMFASecretModel
    {
        public string MFAAccount { get; set; }

        public string MFAIssuer { get; set; }
        public string MFASecret { get; set; }
        public string CreatedBy { get; set; }
    }
}
