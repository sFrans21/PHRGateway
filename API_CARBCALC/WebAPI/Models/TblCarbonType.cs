using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_CarbonType")]
    public partial class TblCarbonType
    {
        [Key]
        public int TypeId { get; set; }
        [StringLength(50)]
        public string TypeName { get; set; }
    }
}
