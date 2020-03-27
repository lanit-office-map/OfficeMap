using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OfficeService.Database.Entities
{
    public partial class OfficeServiceDbContext : DbContext
    {
        public OfficeServiceDbContext()
        {
        }

        public OfficeServiceDbContext(DbContextOptions<OfficeServiceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Offices> Offices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=DOM;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Offices>(entity =>
            {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
