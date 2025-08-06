using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_Tools
    {
        [Key]
        public int ToolsID { get; set; }
        public string Tools { get; set; }
    }
}
