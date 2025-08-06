using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIWHD.Models
{
    [Table("Superapp_CalendarOfEvents")]
    public class CalendarOfEvents
    {

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location{ get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string AllDayEvent { get; set; }
        public string Recurrence { get; set; }
        public string Attachments { get; set; }
    }
}
