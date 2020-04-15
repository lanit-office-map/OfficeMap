using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpaceService.Database.Entities
{
    public partial class SpaceServiceDbContext : DbContext
    {
        public SpaceServiceDbContext()
        {
        }

        public SpaceServiceDbContext(DbContextOptions<SpaceServiceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbMap> MapFiles { get; set; }
        public virtual DbSet<DbOffice> Offices { get; set; }
        public virtual DbSet<DbSpaceType> SpaceTypes { get; set; }
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
            modelBuilder.Entity<DbMap>(entity =>
            {
                entity.HasKey(e => e.MapId)
                    .HasName("PK_Maps");

                entity.Property(e => e.MapId).ValueGeneratedOnAdd();


                entity.Property(e => e.Content)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Extension)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MapGuid)
                    .HasColumnName("MapGUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DbOffice>(entity =>
            {
                entity.HasKey(e => e.OfficeId);

                entity.Property(e => e.OfficeId).ValueGeneratedOnAdd();

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

            modelBuilder.Entity<DbSpaceType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SpaceTypeGuid)
                    .HasColumnName("SpaceTypeGUID")
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<DbSpace>(entity =>
            {
                entity.HasKey(e => e.SpaceId)
                    .HasName("PK__Spaces");

                entity.Property(e => e.SpaceId).ValueGeneratedOnAdd();

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
