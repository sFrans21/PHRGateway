using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_Tree")]
    public partial class TblTree
    {
        //public TblTree()
        //{
        //    TblTree = new HashSet<TblTree>();
        //}

        [Key]
        public int TreeId { get; set; }
        [Required]
        [StringLength(50)]
        public string TreeName { get; set; }
        [Column("img")]
        [StringLength(1000)]
        public string Img { get; set; }
        public double CarbonAbs { get; set; }


        [InverseProperty("Tree")]
        public virtual ICollection<TblTCarbonAbsorption> TblTCarbonAbsorption { get; set; }
    }
}
