using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_Users
    {
        [Key]
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string? name { get; set; }
        public string user_email { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? update_date { get; set; }
        public string? created_by { get; set; }
        public string? update_by { get; set; }
        public bool? is_active { get; set; }
    }
}
