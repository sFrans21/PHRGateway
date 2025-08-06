using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class VwWellForDDL2
    {
        [Key]
        public long ID { get; set; }
        public int ActivityID { get; set; }
        public int WellID { get; set; }
        public string WellName { get; set; }
        public string Tanggal { get; set; }
        public string ACTIVITY_TYPE { get; set; }
        public string WELL { get; set; }
        public int FIELD_ID { get; set; }
    }
}
