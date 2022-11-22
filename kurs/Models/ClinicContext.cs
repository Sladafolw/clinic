using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace kurs.Models
{
    public partial class ClinicContext : DbContext
    {
        public ClinicContext()
        {
        }

        public ClinicContext(DbContextOptions<ClinicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Diagnosis> Diagnoses { get; set; } = null!;
        public virtual DbSet<HospitalRoom> HospitalRooms { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<PatientInHospitalRoom> PatientInHospitalRooms { get; set; } = null!;
        public virtual DbSet<TransferToAnotherRoom> TransferToAnotherRooms { get; set; } = null!;
        public virtual DbSet<Treatment> Treatments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("workstation id=ClinicBase.mssql.somee.com;packet size=4096;user id=Sladafolw_SQLLogin_4;pwd=gizc9q2hj1;data source=ClinicBase.mssql.somee.com;persist security info=False;initial catalog=ClinicBase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(e => e.NumberDiagnosis)
                    .HasName("PK__Diagnosi__39A857D4D58E906A");

                entity.ToTable("Diagnosis");

                entity.Property(e => e.NameDiagnosis).HasMaxLength(100);
            });

            modelBuilder.Entity<HospitalRoom>(entity =>
            {
                entity.HasKey(e => e.NumberHospitalRoom)
                    .HasName("PK__Hospital__60DF137B15F88A10");

                entity.ToTable("HospitalRoom");

                entity.Property(e => e.NumberHospitalRoom).ValueGeneratedNever();
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.CodePatient)
                    .HasName("PK__Patient__357AE8A06C66841A");

                entity.ToTable("Patient");

                entity.Property(e => e.ColorHair).HasMaxLength(20);

                entity.Property(e => e.Gender).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(420);

                entity.Property(e => e.SpecialSigns).HasMaxLength(100);
            });

            modelBuilder.Entity<PatientInHospitalRoom>(entity =>
            {
                entity.HasKey(e => e.CodePatient)
                    .HasName("PK__PatientI__357AE8A068C11F55");

                entity.ToTable("PatientInHospitalRoom");

                entity.Property(e => e.CodePatient).ValueGeneratedNever();

                entity.HasOne(d => d.CodePatientNavigation)
                    .WithOne(p => p.PatientInHospitalRoom)
                    .HasForeignKey<PatientInHospitalRoom>(d => d.CodePatient)
                    .HasConstraintName("R_36");

                entity.HasOne(d => d.NumberHospitalRoomNavigation)
                    .WithMany(p => p.PatientInHospitalRooms)
                    .HasForeignKey(d => d.NumberHospitalRoom)
                    .HasConstraintName("R_34");
            });

            modelBuilder.Entity<TransferToAnotherRoom>(entity =>
            {
                entity.HasKey(e => e.CodeTransfer)
                    .HasName("PK__Transfer__637ADE84D92031FF");

                entity.ToTable("TransferToAnotherRoom");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.CodePatientNavigation)
                    .WithMany(p => p.TransferToAnotherRooms)
                    .HasForeignKey(d => d.CodePatient)
                    .HasConstraintName("R_61");

                entity.HasOne(d => d.NumberHospitalRoomNavigation)
                    .WithMany(p => p.TransferToAnotherRooms)
                    .HasForeignKey(d => d.NumberHospitalRoom)
                    .HasConstraintName("R_39");
            });

            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.HasKey(e => e.TreatmentCode)
                    .HasName("PK__Treatmen__199F1D8B53F5F5E5");

                entity.ToTable("Treatment");

                entity.Property(e => e.DischargeDate).HasColumnType("datetime");

                entity.Property(e => e.DischargeReason).HasMaxLength(500);

                entity.Property(e => e.HowDeliveredPatient).HasMaxLength(100);

                entity.Property(e => e.ReceiptDate).HasColumnType("datetime");

                entity.HasOne(d => d.CodePatientNavigation)
                    .WithMany(p => p.Treatments)
                    .HasForeignKey(d => d.CodePatient)
                    .HasConstraintName("R_30");

                entity.HasOne(d => d.NumberDiagnosisNavigation)
                    .WithMany(p => p.Treatments)
                    .HasForeignKey(d => d.NumberDiagnosis)
                    .HasConstraintName("R_70");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

