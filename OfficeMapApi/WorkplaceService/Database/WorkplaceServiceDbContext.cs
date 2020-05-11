using Microsoft.EntityFrameworkCore;
using WorkplaceService.Database.Entities;

namespace WorkplaceService.Database
{
    public partial class WorkplaceServiceDbContext : DbContext
    {
        public WorkplaceServiceDbContext()
        {
        }

        public WorkplaceServiceDbContext(DbContextOptions<WorkplaceServiceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbMapFile> MapFiles { get; set; }
        public virtual DbSet<DbWorkplace> Workplaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=DOM;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbMapFile>(entity =>
            {
                entity.HasKey(e => e.MapId)
                    .HasName("PK_Maps");

                entity.Property(e => e.MapId).ValueGeneratedOnAdd();

                entity.Property(e => e.Content)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Guid)
                    .HasColumnName("MapGUID")
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<DbWorkplace>(entity =>
            {
                entity.HasKey(e => e.WorkplaceId)
                    .HasName("PK_Workplace");

                entity.Property(e => e.WorkplaceId).ValueGeneratedOnAdd();

                entity.Property(e => e.Guid)
                    .HasColumnName("WorkplaceGUID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Map)
                    .WithOne(p => p.Workplace)
                    .HasForeignKey<DbWorkplace>(w => w.MapId)
                    .HasConstraintName("FK_Workplaces_MapFiles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
