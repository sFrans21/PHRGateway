using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_Activity
    {
        [Key]
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
    }
}
