using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Database.Entities
{
    public class DbUser : IdentityUser
    {
        public int EmployeeId { get; set; }

        public virtual DbEmployee Employee { get; set; }
        [NotMapped]
        public object UserId { get; internal set; }

        public DbUser()
        {
        }
    }
}
