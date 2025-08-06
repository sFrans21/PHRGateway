using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Models
{
    public class VwWellReport
    {
        [Key]
        public long ID { get; set; }
        public int TransactionID { get; set; }
        public string? FileName { get; set; }
        public string NamaSumur { get; set; }
        public string Lokasi { get; set; }
        public string? Tanggal { get; set; }
        public DateTime? Tanggal2 { get; set; }
        public string? Waktu { get; set; }
        public string Kategori { get; set; }
        public string? Uploader { get; set; }
        public string NamaPeralatan { get; set; }
        public string Keterangan { get; set; }
        public string? TerakhirDiUbah { get; set; }
        public string? DiubahOleh { get; set; }
    }
}
