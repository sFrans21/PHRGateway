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
    [Table("Tbl_UsersTest")]
    public partial class TblTUsersTest
    {
        

        [Key]        
        public int userID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string UserName { get; set; }

        public int Period { get; set; }
        public string Email { get; set; }
        
        public string Regional { get; set; }
        public string Zona { get; set; }
        public string Field { get; set; }
        public int IsAdmin { get; set; }
        public int IsFirstLogin { get; set; }



    }
}
