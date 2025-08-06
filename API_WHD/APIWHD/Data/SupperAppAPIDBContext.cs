using APIWHD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APIWHD.Data
{
    public class SupperAppAPIDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public SupperAppAPIDBContext()
        {

        }

        public SupperAppAPIDBContext(DbContextOptions<SupperAppAPIDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Broadcast> Broadcasts { get; set; }
        public virtual DbSet<CalendarOfEvents> CalendarOfEvents { get; set; }
        public virtual DbSet<CoorporateNews> CoorporateNews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // You can further configure the view here if needed.
            modelBuilder.Entity<Broadcast>(entity =>
            {
                entity.HasNoKey(); // Configure this entity as keyless
                //entity.ToView("VwGetWellByRole"); // Replace with your actual view name.
            });
        }
    }
}
