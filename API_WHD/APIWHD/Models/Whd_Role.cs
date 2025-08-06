using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class Whd_Role
    {
        [Key]
        public int RoleID { get; set; }
        public string Role { get; set; }
    }
}
