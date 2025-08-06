using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_DocumentsTrans
    {
        [Key]
        public int DocumentsTransID { get; set; }
        public int TransactionID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
