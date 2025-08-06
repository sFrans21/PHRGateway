using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.ResponseModels
{
    public class AimanEmployeeData
    {
        public bool Status { get; set; }
        public List<AimanEmployeeInfo> Object { get; set; }
        public string Message { get; set; }
    }
}
