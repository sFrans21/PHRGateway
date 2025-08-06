using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_Field
    {
        [Key]
        public long field_id { get; set; }
        public long? area_id { get; set; }
        public string? field_name { get; set; }
        public string? field_alias_name { get; set; }
        public string? percentage_share { get; set; }
        public bool is_active { get; set; }
    }
}
