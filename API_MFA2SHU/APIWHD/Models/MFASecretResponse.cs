using System.Collections.Generic;

namespace APIWHD.Models
{
    public class MFASecretResponse
    {
        public bool Status { get; set; }
        public List<MFASecretObject> Object { get; set; }
        public string Message { get; set; }
    }
}
