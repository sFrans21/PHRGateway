using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Vw_VehicleEmision_SUM_Year")]
    public partial class VwVehicleEmision_SUM_Year
    {
        [Key]
        public int UserID { get; set; }
        public int Tahun { get; set; }
        public Decimal EmisiTahun { get; set; }

    }
}
