using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Vw_Periode")]
    public partial class VwPeriode
    {
        [Key]
        public int PeriodeId { get; set; }
        public int UserId { get; set; }
        public int MonthValue { get; set; }
        public int YearValue { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }

    }
}
