using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class VwGetWellByRole
    {
        public int ActivityID { get; set; }
        public int? WellID { get; set; }
        public string? WELL { get; set; }
        public string User_Name { get; set; }
    }
}
