using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSystem.Models
{
    public partial class VwPeriode
    {
        [Key]
        public int PeriodeId { get; set; }
        public int UserId { get; set; }
        public int MonthValue { get; set; }
        public int YearValue { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
