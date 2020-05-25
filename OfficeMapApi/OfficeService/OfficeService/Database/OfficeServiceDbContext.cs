using Microsoft.EntityFrameworkCore;
using OfficeService.Database.Entities;

namespace OfficeService.Database
{
    public partial class OfficeServiceDbContext : DbContext
    {
        public OfficeServiceDbContext(DbContextOptions<OfficeServiceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbOffice> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbOffice>(entity =>
            {
                entity.ToTable("Offices", "dbo");
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
                entity.HasQueryFilter(e => e.Obsolete == false);
            });
        }
    }
}