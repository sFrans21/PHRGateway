using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_HouseholdCalc")]
    public partial class TblHouseholdCalc
    {
        [Key]
        public int HouseholdCalcId { get; set; }
        public int? HouseholdId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ElectricityCons { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GasCons { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ElectricityEmision { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GasEmision { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PeopleEmision { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        [InverseProperty(nameof(TblTHousehold.TblHouseholdCalc))]
        public virtual TblTHousehold Household { get; set; }
    }
}
