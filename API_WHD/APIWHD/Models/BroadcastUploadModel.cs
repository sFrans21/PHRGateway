using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class BroadcastUploadModel
    {
        public IFormFile files { get; set; }
    }
}
