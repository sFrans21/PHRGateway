using APIWHD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWHD.Data
{
    public class APIDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public APIDBContext()
        {

        }

        public APIDBContext(DbContextOptions<APIDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Whd_Category> Whd_Category { get; set; }
        public virtual DbSet<VwWellForDDL2> VwWellForDDL2 { get; set; }
        public virtual DbSet<Whd_Tools> Whd_Tools { get; set; }
        public virtual DbSet<VwGetWellByRole> VwGetWellByRole { get; set; }
        public virtual DbSet<TransactionDashboardItem> TransactionDashboardItems { get; set; }
        public virtual DbSet<VwReportAll> VwReportAll { get; set; }
        public virtual DbSet<VwDashMyWHD> VwDashMyWHD { get; set; }
        public virtual DbSet<Whd_Activity> Whd_Activity { get; set; }
        public virtual DbSet<Whd_Transaction> Whd_Transaction { get; set; }
        public virtual DbSet<Whd_DocumentsTrans> Whd_DocumentsTrans { get; set; }
        public virtual DbSet<VwWellReport> VwWellReport { get; set; }
        public virtual DbSet<Whd_Role> Whd_Role { get; set; }
        public virtual DbSet<Whd_Field> Whd_Field { get; set; }
        public virtual DbSet<Whd_Type> Whd_Type { get; set; }
        public virtual DbSet<Whd_Users> Whd_Users { get; set; }
        public virtual DbSet<GetUserRoleMobile> GetUserRoleMobile { get; set; }
        //public virtual DbSet<Broadcast> Broadcasts { get; set; }
        public virtual DbSet<CalendarOfEvents> CalendarOfEvents { get; set; }
        public virtual DbSet<NewsBroadcast> NewsBroadcasts { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetail { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // You can further configure the view here if needed.
            modelBuilder.Entity<VwGetWellByRole>(entity =>
            {
                entity.HasNoKey(); // Configure this entity as keyless
                entity.ToView("VwGetWellByRole"); // Replace with your actual view name.
            });
            // You can further configure the view here if needed.
            modelBuilder.Entity<VwReportAll>(entity =>
            {
                entity.HasNoKey(); // Configure this entity as keyless
                entity.ToView("VwReportAll"); // Replace with your actual view name.
            });
            modelBuilder.Entity<GetUserRoleMobile>(entity =>
            {
                entity.HasNoKey(); // Configure this entity as keyless
                entity.ToView("GetUserRoleMobile"); // Replace with your actual view name.
            });
        }
    }
}
