using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class VwReportAll
    {
        public int TransactionID { get; set; }
        public string Well_Short_Name { get; set; }
        public string CategoryDetailDesc { get; set; }
        public string Tools { get; set; }
        public string ActivityName { get; set; }
        public DateTime? RealtimePelaporan { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
    }
}
