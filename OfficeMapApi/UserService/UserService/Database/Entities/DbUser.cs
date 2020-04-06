using Microsoft.AspNetCore.Identity;

namespace UserService.Database.Entities
{
    public class DbUser : IdentityUser
    {
        public int EmployeeId { get; set; }

        public virtual DbEmployee Employee { get; set; }

        public DbUser()
        {
        }
    }
}
