using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_Type
    {
        [Key]
        public int TypeID { get; set; }
        public string? TypeName { get; set; }
        public string? Type { get; set; }
        public int? ParentTypeID { get; set; }
    }
}
