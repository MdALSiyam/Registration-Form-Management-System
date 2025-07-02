using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RegistrationApi.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<Personeel> Personeels { get; set; }

    public virtual DbSet<TableCourseDetail> TableCourseDetails { get; set; }

    public virtual DbSet<TblRegistration> TblRegistrations { get; set; }

    public virtual DbSet<ViwRegistration> ViwRegistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-OL6FUR8;Database=Manager;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Sl);

            entity.ToTable("Announcement");

            entity.Property(e => e.Sl).HasColumnName("SL");
            entity.Property(e => e.Banner).HasColumnType("image");
            entity.Property(e => e.CourseSl).HasColumnName("CourseSL");
            entity.Property(e => e.DateEnd).HasColumnType("smalldatetime");
            entity.Property(e => e.DateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.Days)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EarlyBird).HasColumnType("smalldatetime");
            entity.Property(e => e.EntryBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Fees).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.LastDateOfReg).HasColumnType("smalldatetime");
            entity.Property(e => e.Rpsl).HasColumnName("RPSL");
            entity.Property(e => e.TimeEnd).HasColumnType("smalldatetime");
            entity.Property(e => e.TimeFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.TotalHrs)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Venue).IsUnicode(false);
        });

        modelBuilder.Entity<Personeel>(entity =>
        {
            entity.HasKey(e => e.Sl);

            entity.ToTable("Personeel");

            entity.Property(e => e.Sl).HasColumnName("SL");
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EMail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("eMail");
            entity.Property(e => e.EntryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LastAccessDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LastUpdate).HasColumnType("smalldatetime");
            entity.Property(e => e.Organization)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pName");
            entity.Property(e => e.PType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pType");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PicPath)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Qualification).IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TableCourseDetail>(entity =>
        {
            entity.HasKey(e => e.Sl);

            entity.Property(e => e.Sl).HasColumnName("SL");
            entity.Property(e => e.Boarding).IsUnicode(false);
            entity.Property(e => e.CategorySl).HasColumnName("CategorySL");
            entity.Property(e => e.CourseContent).IsUnicode(false);
            entity.Property(e => e.CourseName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CourseType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EntryBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Methology).IsUnicode(false);
            entity.Property(e => e.OutComes).IsUnicode(false);
            entity.Property(e => e.WhoCanAttend).IsUnicode(false);
        });

        modelBuilder.Entity<TblRegistration>(entity =>
        {
            entity.HasKey(e => e.Sl);

            entity.ToTable("tblRegistration");

            entity.Property(e => e.DRegistrationDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("dRegistrationDate");
            entity.Property(e => e.IAnnouncementSl).HasColumnName("iAnnouncementSL");
            entity.Property(e => e.IFees).HasColumnName("iFees");
            entity.Property(e => e.IPersoneelSl).HasColumnName("iPersoneelSL");
            entity.Property(e => e.SPaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sPaymentMethod");
            entity.Property(e => e.SStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sStatus");
            entity.Property(e => e.VCategory)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("vCategory");
            entity.Property(e => e.VEntryBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vEntryBy");
            entity.Property(e => e.VPaymentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vPaymentType");
            entity.Property(e => e.VTrxId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("vTrxID");
        });

        modelBuilder.Entity<ViwRegistration>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viwRegistration");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CourseName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CourseSl).HasColumnName("CourseSL");
            entity.Property(e => e.CourseType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DRegistrationDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("dRegistrationDate");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EMail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("eMail");
            entity.Property(e => e.IAnnouncementSl).HasColumnName("iAnnouncementSL");
            entity.Property(e => e.IFees).HasColumnName("iFees");
            entity.Property(e => e.IPersoneelSl).HasColumnName("iPersoneelSL");
            entity.Property(e => e.Organization)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pName");
            entity.Property(e => e.PType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pType");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Qualification).IsUnicode(false);
            entity.Property(e => e.SPaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sPaymentMethod");
            entity.Property(e => e.SStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sStatus");
            entity.Property(e => e.VCategory)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("vCategory");
            entity.Property(e => e.VEntryBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vEntryBy");
            entity.Property(e => e.VPaymentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vPaymentType");
            entity.Property(e => e.VTrxId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("vTrxID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
