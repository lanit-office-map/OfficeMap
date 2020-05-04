using Microsoft.EntityFrameworkCore;
using OfficeService.Database.Entities;

namespace OfficeService.Database
{
    public partial class OfficeServiceDbContext : DbContext
    {
        public OfficeServiceDbContext(DbContextOptions<OfficeServiceDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
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

                entity.Property(e => e.Guid)
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
                entity.ToTable("Spaces", "dbo");
                entity.HasKey(e => e.SpaceId)
                    .HasName("PK__Spaces");

                entity.Property(e => e.SpaceId).ValueGeneratedOnAdd();

                entity.Property(e => e.SpaceGuid)
                    .HasColumnName("SpaceGUID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Spaces)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
