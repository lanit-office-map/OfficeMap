using Microsoft.EntityFrameworkCore;
using WorkplaceService.Database.Entities;

namespace WorkplaceService.Database
{
  public partial class WorkplaceServiceDbContext : DbContext
  {
    public WorkplaceServiceDbContext(DbContextOptions<WorkplaceServiceDbContext> options)
      : base(options)
    {
    }


    public virtual DbSet<DbWorkplace> Workplaces { get; set; }
    public virtual DbSet<DbMapFile> MapFiles { get; set; }

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
        entity.HasQueryFilter(e => e.Obsolete == false);
      });

      modelBuilder.Entity<DbWorkplace>(entity =>
      {
        entity.HasKey(e => e.WorkplaceId)
          .HasName("PK_Workplace");

        entity.Property(e => e.WorkplaceId).ValueGeneratedOnAdd();

        entity.Property(e => e.WorkplaceGuid)
          .HasColumnName("WorkplaceGUID")
          .HasDefaultValueSql("(newid())");

        entity.HasOne(d => d.Map)
          .WithOne(p => p.Workplace)
          .HasForeignKey<DbWorkplace>(w => w.MapId)
          .HasConstraintName("FK_Workplaces_MapFiles");

        entity.HasOne(d => d.Space)
          .WithOne()
          .HasForeignKey<DbWorkplace>(w => w.SpaceId)
          .HasConstraintName("FK_Workplaces_Spaces_SpaceId");

        entity.HasQueryFilter(e => e.Obsolete == false);
      });

      modelBuilder.Entity<DbSpace>(
        entity =>
        {
          entity.ToTable("Spaces", "dbo");

          entity.HasKey(e => e.SpaceId)
            .HasName("PK_Spaces");
        });
    }
  }
}
