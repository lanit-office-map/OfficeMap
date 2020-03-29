using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserServiceApi.Database.Entities;

namespace UserServiceApi.Database
{
    public class UserServiceDBContext : IdentityDbContext<DbUser>
    {
        public UserServiceDBContext(DbContextOptions<UserServiceDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<DbEmployee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbUser>(entity =>
            {
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DbEmployee>(entity =>
            {
                entity.ToTable("Employees");

                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();

                entity.Property(e => e.EmployeeGuid)
                    .HasColumnName("EmployeeGUID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}