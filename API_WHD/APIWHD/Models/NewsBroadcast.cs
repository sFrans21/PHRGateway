using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIWHD.Models
{
    [Table("Superapp_CorporateNews")]
    public class NewsBroadcast
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Tanggal { get; set; }
        public string? Teaser { get; set; }
        public string? PageImage { get; set; }
        public string? Thumbnail { get; set; }
        public string? ContentNews { get; set; }
        public string? Authors { get; set; }
    }
}
