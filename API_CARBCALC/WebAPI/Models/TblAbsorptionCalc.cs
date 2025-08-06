using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_AbsorptionCalc")]
    public partial class TblAbsorptionCalc
    {
        [Key]
        public int AbsorptionCalcId { get; set; }
        public int? CarbonAbsorptionId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AbsorptionPower { get; set; }

        [ForeignKey(nameof(CarbonAbsorptionId))]
        [InverseProperty(nameof(TblTCarbonAbsorption.TblAbsorptionCalc))]
        public virtual TblTCarbonAbsorption CarbonAbsorption { get; set; }
    }
}
