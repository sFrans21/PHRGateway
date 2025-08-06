using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_Periode")]
    public partial class TblPeriode
    {
        public TblPeriode()
        {
            TblTCarbonAbsorption = new HashSet<TblTCarbonAbsorption>();
            TblTHousehold = new HashSet<TblTHousehold>();
            TblTVehicleEmision = new HashSet<TblTVehicleEmision>();
        }

        [Key]
        public int PeriodeId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Month { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Year { get; set; }

        [InverseProperty("Periode")]
        public virtual ICollection<TblTCarbonAbsorption> TblTCarbonAbsorption { get; set; }
        [InverseProperty("Periode")]
        public virtual ICollection<TblTHousehold> TblTHousehold { get; set; }
        [InverseProperty("Periode")]
        public virtual ICollection<TblTVehicleEmision> TblTVehicleEmision { get; set; }
        public virtual ICollection<VwVehicleList_User> VwVehicleList_User { get; set; }
        public virtual ICollection<VwVehicleList_UserSum> VwVehicleList_UserSum { get; set; }
        public virtual ICollection<VwHouseholdbyYear> VwHouseholdbyYear { get; set; }
    }
}
