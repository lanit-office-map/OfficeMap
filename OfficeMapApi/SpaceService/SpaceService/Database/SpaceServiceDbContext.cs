using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<DbSpace> Spaces { get; set; }
        public virtual DbSet<DbMapFile> MapFiles { get; set; }
        public virtual DbSet<DbSpaceType> SpaceTypes { get; set; }

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

                entity.Property(e => e.MapGuid)
                    .HasColumnName("MapGUID")
                    .HasDefaultValueSql("(newid())");

                entity.HasQueryFilter(s => s.Obsolete == false);
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

                entity.HasQueryFilter(s => s.Obsolete == false);
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

                entity.HasOne(d => d.MapFile)
                    .WithOne()
                    .HasForeignKey<DbSpace>(d => d.MapId);

                entity.HasMany(s => s.Spaces)
                    .WithOne(s => s.Parent)
                    .HasForeignKey(s => s.ParentId);

                entity.HasOne(d => d.SpaceType)
                    .WithOne()
                    .HasForeignKey<DbSpace>(d => d.TypeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasQueryFilter(s => s.Obsolete == false);
            });
        }
    }
}
