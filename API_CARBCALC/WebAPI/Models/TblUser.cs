using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_Users")]
    public partial class TblTUser
    {
        

        [Key]        
        public int userID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string UserName { get; set; }

        public int Period { get; set; }
        public string Email { get; set; }


        public decimal? HouseCalculation { get; set; }
        public decimal? VehicleCalculation { get; set; }
        public decimal? CarbonAbsCalculation { get; set; }
        public decimal? TotalEmision { get; set; }
        public string? Regional { get; set; }
        public string? Zona { get; set; }
        public string? Field { get; set; }
        public int? IsAdmin { get; set; }
        public int? IsFirstLogin { get; set; }

        public string IsAdminText()
        {
            string AdminText;

            switch (IsAdmin)
            {
                case 1:
                    AdminText = "Super Admin";
                    break;

                case 2: AdminText =  "Admin Regional";
                    break;                   

            }

            return ("test");

        }

    }
}
