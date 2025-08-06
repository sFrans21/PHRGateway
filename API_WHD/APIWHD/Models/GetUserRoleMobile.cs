using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class GetUserRoleMobile
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public int role_id { get; set; }
        public string Role { get; set; }
        public int? location_id { get; set; }
        public string? field_name { get; set; }
        public int? type_role_id { get; set; }
        public string? Typename { get; set; }
        public int? ParentTypeID { get; set; }
        public bool is_active { get; set; }
    }
}
