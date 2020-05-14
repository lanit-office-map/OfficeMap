using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserService.Database.Entities;

namespace UserService.Database
{
    public class UserServiceDbContext : ApiAuthorizationDbContext<DbUser>
    {
        public UserServiceDbContext(
            DbContextOptions<UserServiceDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public virtual DbSet<DbEmployee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbUser>(entity =>
            {
                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.User)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DbEmployee>(entity =>
            {
                entity.ToTable("Employees", "dbo");

                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();

                entity.Property(e => e.EmployeeGuid)
                    .HasColumnName("EmployeeGUID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(e => e.User)
                    .WithOne(au => au.Employee)
                    .HasForeignKey<DbUser>(u => u.EmployeeId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}