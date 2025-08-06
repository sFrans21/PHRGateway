using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class UploadModel
    {
        public string username { get; set; }
        public IFormFile files { get; set; }
        public int transactionID { get; set; }
    }
}
