using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSystem.Models
{
    public partial class VwVehicleEmision_SUM_Month
    {
        [Key]
        public int UserID { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; }
        public Decimal EmisiBulan { get; set; }

    }
}
