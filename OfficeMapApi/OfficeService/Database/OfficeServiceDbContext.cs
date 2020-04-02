using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OfficeService.Database.Entities;

namespace OfficeService.Database
{
    public partial class OfficeServiceDbContext : DbContext 
    {
        public OfficeServiceDbContext (DbContextOptions<OfficeServiceDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }       

        public virtual DbSet<DbOffice> Offices { get; set; } 
        public virtual DbSet<DbSpace> Spaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=DOM;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            
            modelBuilder.Entity<DbOffice>(entity =>
            {
                entity.ToTable("Offices");

                entity.HasKey(e => e.OfficeId);

                entity.Property(e => e.OfficeId).ValueGeneratedNever();

                entity.Property(e => e.Building)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.House)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OfficeGuid)
                    .HasColumnName("OfficeGUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("Phone_Number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            }); 

            modelBuilder.Entity<DbSpace>(entity =>
            {
                entity.ToTable("Spaces");
                entity.HasKey(e => e.SpaceId)
                    .HasName("PK__Spaces");

                entity.Property(e => e.SpaceId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpaceGuid)
                    .HasColumnName("SpaceGUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.SpaceName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.Spaces)
                    .HasForeignKey(d => d.MapId);

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Spaces)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Spaces)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
