using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.ResponseModels
{
    public class TokenResponseModel
    {
        public string access_token { get; set; }
        public int? expires_in { get; set; }
        public int? refresh_expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string id_token { get; set; }
        public string session_state { get; set; }
        [Display(Name = "not-before-policy")]
        public int? not_before_policy { get; set; }
        public string scope { get; set; }
    }
}
