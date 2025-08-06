using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class VwDashMyWHD
    {
        [Key]
        public int ID { get; set; }
        public int Well { get; set; }        
        public int Dokumen { get; set; }
        public int Kategori { get; set; }
        public int Users { get; set; }
    }
}
