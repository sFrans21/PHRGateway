using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class TransactionDashboardItem
    {
        public long ID { get; set; }
        public int TransactionId { get; set; }
        public string Well_Short_Name { get; set; }
        public string? Filename { get; set; }
        public string Tools { get; set; }
        public string? ActivityName { get; set; }
        public string CategoryDesc { get; set; }
        public string RealtimePelaporan { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Description { get; set; }
    }
}
