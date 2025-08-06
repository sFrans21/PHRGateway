using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebSystem.Models
{
    public partial class TblFuel
    {

        [Key]
        public int FuelId { get; set; }
        public string FuelName { get; set; }
        public decimal Co2_KG { get; set; }

    }

}
