using MapService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace MapService.Database
{
    public partial class MapServiceDbContext : DbContext
    {
        public MapServiceDbContext(DbContextOptions<MapServiceDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<DbMapFiles> MapFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbMapFiles>(entity =>
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
