using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAPI.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable 

namespace WebAPI.Data
{
    public partial class APIDBContext : DbContext
    {
        public APIDBContext()
        {
        }

        public APIDBContext(DbContextOptions<APIDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAbsorptionCalc> TblAbsorptionCalc { get; set; }
        public virtual DbSet<TblCarbonType> TblCarbonType { get; set; }
        public virtual DbSet<TblFuel> TblFuel { get; set; }
        public virtual DbSet<VwFuel> VwFuel { get; set; }
        public virtual DbSet<TblHouseholdCalc> TblHouseholdCalc { get; set; }
        public virtual DbSet<TblPeriode> TblPeriode { get; set; }
        public virtual DbSet<TblTCarbonAbsorption> TblTCarbonAbsorption { get; set; }
        public virtual DbSet<TblTCarbonAbsorptionViewModel_Sum> TblTCarbonAbsorptionViewModel { get; set; }
        public virtual DbSet<TblTHousehold> TblTHousehold { get; set; }
        public virtual DbSet<TblTVehicleEmision> TblTVehicleEmision { get; set; }
        public virtual DbSet<TblTransportation> TblTransportation { get; set; }
        public virtual DbSet<TblTree> TblTree { get; set; }
        public virtual DbSet<TblVehicle> TblVehicle { get; set; }
        public virtual DbSet<TblVehicleCalc> TblVehicleCalc { get; set; }
        public virtual DbSet<TblVehicleCapacity> TblVehicleCapacity { get; set; }
        public virtual DbSet<TblVehicleCategory> TblVehicleCategory { get; set; }
        public virtual DbSet<TblVehicleType> TblVehicleType { get; set; }

        public virtual DbSet<VwVehicleList_User> VwVehicleList_User { get; set; }
        public virtual DbSet<VwVehicleList_UserSum> VwVehicleList_UserSum { get; set; }
        public virtual DbSet<VwVehicleList> VwVehicleList { get; set; }
        public virtual DbSet<VwVehicleList_User_Action> VwVehicleList_User_Action { get; set; }
        public virtual DbSet<VwPeriode> VwPeriode { get; set; }
        public virtual DbSet<FnGetSummary> FnGetSummary { get; set; }
        public virtual DbSet<VwPeriodeListSummary> VwPeriodeListSummary { get; set; }
        public virtual DbSet<VwEmission_Report> VwEmission_Report { get; set; }
        public virtual DbSet<Vw_Emission_Report_Field> VwEmission_Report_Field { get; set; }
        public virtual DbSet<VwHouseholdbyYear> VwHouseholdbyYear { get; set; }

        public virtual DbSet<FNGetVehicleEmision_Calc> FNGetVehicleEmision_Calc { get; set; }
        public virtual DbSet<VwVehicleEmision_SUM_Year> VwVehicleEmision_SUM_Year { get; set; }
        public virtual DbSet<VwVehicleEmision_SUM_Month> VwVehicleEmision_SUM_Month { get; set; }
        public virtual DbSet<TblTCarbonAbsorptionViewModel_Sum> TblTCarbonAbsorptionViewModel_Sum { get; set; }
        public virtual DbSet<TblTCarbonAbsorptionViewModel_SumYear> TblTCarbonAbsorptionViewModel_SumYear { get; set; }
        public virtual DbSet<TblMasterCompany> TblMasterCompany { get; set; }
        public virtual DbSet<TblMasterRegional> TblMasterRegional { get; set; }
        public virtual DbSet<TblMasterZona> TblMasterZona { get; set; }
        public virtual DbSet<TblTUser> TblTUser { get; set; }
        public virtual DbSet<TblMasterField> TblMasterField { get; set; }
        public virtual DbSet<TblTUsersTest> TblTUsersTest { get; set; }
        public virtual DbSet<VwMasterVehicleFormula> VwMasterVehicleFormula { get; set; }
        public virtual DbSet<VwVehicleList_AddFormula> VwVehicleList_AddFormula { get; set; }
        public virtual DbSet<Execute_SP_Output> Execute_SP_Output { get; set; }
        public virtual DbSet<Vw_HouseholdSumListmonth> Vw_HouseholdSumListmonth { get; set; }
        public virtual DbSet<Vw_HouseholdSumListYear> Vw_HouseholdSumListYear { get; set; }
        public virtual DbSet<VwPeriodeListSummary_Chart> VwPeriodeListSummary_Chart { get; set; }
        public virtual DbSet<VwAIMAN_Username> VwAIMAN_Username { get; set; }


        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=PHR-PF3HQMHQ;Database=api_dev2;Trusted_Connection=True;MultipleActiveResultSets=True;");
        //            }
        //        }

        //    protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //        modelBuilder.Entity<TblAbsorptionCalc>(entity =>
        //        {
        //            entity.HasKey(e => e.AbsorptionCalcId)
        //                .HasName("PK__Tbl_Abso__8D4F15800CB56DB7");

        //            entity.HasOne(d => d.CarbonAbsorption)
        //                .WithMany(p => p.TblAbsorptionCalc)
        //                .HasForeignKey(d => d.CarbonAbsorptionId)
        //                .HasConstraintName("FK__Tbl_Absor__Carbo__4D94879B");
        //        });

        //        modelBuilder.Entity<TblFuel>(entity =>
        //        {
        //            entity.HasKey(e => e.FuelId)
        //                .HasName("PK__Tbl_Fuel__706CF3E735E12E6D");
        //        });

        //        modelBuilder.Entity<TblHouseholdCalc>(entity =>
        //        {
        //            entity.HasKey(e => e.HouseholdCalcId)
        //                .HasName("PK__Tbl_Hous__151C9B0F64F24260");

        //            entity.HasOne(d => d.Household)
        //                .WithMany(p => p.TblHouseholdCalc)
        //                .HasForeignKey(d => d.HouseholdId)
        //                .HasConstraintName("FK__Tbl_House__House__47DBAE45");
        //        });

        //        modelBuilder.Entity<TblTCarbonAbsorption>(entity =>
        //        {
        //            entity.HasKey(e => e.CarbonAbsorptionId)
        //                .HasName("PK__TblT_Car__9A830D45FF604E48");

        //            entity.HasOne(d => d.Periode)
        //                .WithMany(p => p.TblTCarbonAbsorption)
        //                .HasForeignKey(d => d.PeriodeId)
        //                .HasConstraintName("FK__TblT_Carb__Perio__44FF419A");

        //            entity.HasOne(d => d.Tree)
        //                .WithMany(p => p.TblTCarbonAbsorption)
        //                .HasForeignKey(d => d.TreeId)
        //                .HasConstraintName("FK__TblT_Carb__TreeI__440B1D61");
        //        });

        //        modelBuilder.Entity<TblTHousehold>(entity =>
        //        {
        //            entity.HasOne(d => d.Periode)
        //                .WithMany(p => p.TblTHousehold)
        //                .HasForeignKey(d => d.PeriodeId)
        //                .HasConstraintName("FK__TblT_Hous__Perio__3A81B327");
        //        });

        //        modelBuilder.Entity<TblTVehicleEmision>(entity =>
        //        {
        //            entity.HasKey(e => e.VehicleEmisionId)
        //                .HasName("PK__TblT_Veh__B13FFA124713999E");

        //            entity.HasOne(d => d.Capacity)
        //                .WithMany(p => p.TblTVehicleEmision)
        //                .HasForeignKey(d => d.CapacityId)
        //                .HasConstraintName("FK__TblT_Vehi__Capac__3F466844");

        //            entity.HasOne(d => d.Fuel)
        //                .WithMany(p => p.TblTVehicleEmision)
        //                .HasForeignKey(d => d.FuelId)
        //                .HasConstraintName("FK__TblT_Vehi__FuelI__3D5E1FD2");

        //            entity.HasOne(d => d.Periode)
        //                .WithMany(p => p.TblTVehicleEmision)
        //                .HasForeignKey(d => d.PeriodeId)
        //                .HasConstraintName("FK__TblT_Vehi__Perio__403A8C7D");

        //            entity.HasOne(d => d.Transportation)
        //                .WithMany(p => p.TblTVehicleEmision)
        //                .HasForeignKey(d => d.TransportationId)
        //                .HasConstraintName("FK__TblT_Vehi__Trans__3E52440B");

        //            entity.HasOne(d => d.Vehicle)
        //                .WithMany(p => p.TblTVehicleEmision)
        //                .HasForeignKey(d => d.VehicleId)
        //                .HasConstraintName("FK__TblT_Vehi__Vehic__3C69FB99");
        //        });

        //        modelBuilder.Entity<TblTransportation>(entity =>
        //        {
        //            entity.HasKey(e => e.TransportationId)
        //                .HasName("PK__Tbl_Tran__87E47936F2EC6B7A");
        //        });

        //        modelBuilder.Entity<TblTree>(entity =>
        //        {
        //            entity.HasKey(e => e.TreeId)
        //                .HasName("PK__Tbl_Tree__35F32425E4C211AD");
        //        });

        //        modelBuilder.Entity<TblVehicle>(entity =>
        //        {
        //            entity.HasKey(e => e.VehicleId)
        //                .HasName("PK__Tbl_Vehi__476B5492C2ADB763");

        //            entity.HasOne(d => d.VehicleType)
        //                .WithMany(p => p.TblVehicle)
        //                .HasForeignKey(d => d.VehicleTypeId)
        //                .OnDelete(DeleteBehavior.ClientSetNull)
        //                .HasConstraintName("FK__Tbl_Vehic__Vehic__5070F446");
        //        });

        //        modelBuilder.Entity<TblVehicleCalc>(entity =>
        //        {
        //            entity.HasKey(e => e.VehicleCalcId)
        //                .HasName("PK__Tbl_Vehi__2AD253286D98E3CB");
        //        });

        //        modelBuilder.Entity<TblVehicleCapacity>(entity =>
        //        {
        //            entity.HasKey(e => e.CapacityId)
        //                .HasName("PK__Tbl_Vehi__5AEEAE1A8E411BDB");

        //            entity.HasOne(d => d.Fuel)
        //                .WithMany(p => p.TblVehicleCapacity)
        //                .HasForeignKey(d => d.FuelId)
        //                .HasConstraintName("FK__Tbl_Vehic__FuelI__36B12243");

        //            entity.HasOne(d => d.Transportation)
        //                .WithMany(p => p.TblVehicleCapacity)
        //                .HasForeignKey(d => d.TransportationId)
        //                .HasConstraintName("FK__Tbl_Vehic__Trans__37A5467C");

        //            entity.HasOne(d => d.Vehicle)
        //                .WithMany(p => p.TblVehicleCapacity)
        //                .HasForeignKey(d => d.VehicleId)
        //                .HasConstraintName("FK__Tbl_Vehic__Vehic__35BCFE0A");
        //        });

        //        modelBuilder.Entity<TblVehicleCategory>(entity =>
        //        {
        //            entity.HasKey(e => e.CategoryId)
        //                .HasName("PK__Tbl_vehi__19093A0B656100EC");
        //        });

        //        OnModelCreatingPartial(modelBuilder);
        //    }

        //    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
