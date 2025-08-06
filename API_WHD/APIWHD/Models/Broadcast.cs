using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    [Table("Superapp_FilePath")]
    public class Broadcast
    {
        public int sid { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? HyperLink { get; set; }

    }
}
