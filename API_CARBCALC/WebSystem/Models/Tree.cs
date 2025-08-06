using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models
{
    public class Tree
    {
        [Key]
        public int TreeId { get; set; }
        [Required]
        [StringLength(50)]
        public string TreeName { get; set; }        
        public string Img { get; set; }
        public decimal CarbonAbs { get; set; }

        [Required(ErrorMessage = "Please choose profile image")]
        public IFormFile TreeImage { get; set; }

        //public decimal TreeEmision()
        //{            
        //    return CarbonAbs;
        //}        

    }
}
