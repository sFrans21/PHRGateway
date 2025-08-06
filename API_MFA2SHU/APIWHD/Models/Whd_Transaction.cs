using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int WellID { get; set; }
        public int? CatgoryDetailID { get; set; }
        public int? ToolsID { get; set; }
        public DateTime RealtimePelaporan { get; set; }
        public int? ActivityID { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
