using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebAPI.Models
{
    public class Execute_SP_Output
    {
        [Key]
        public int IsSukses { get; set; }
        public string Info { get; set; }

    
    }
}
