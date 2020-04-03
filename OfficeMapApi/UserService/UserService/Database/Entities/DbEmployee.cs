using System;
using System.Collections.Generic;

namespace UserService.Database.Entities
{
    public partial class DbEmployee
    {
        public DbEmployee()
        {
            Users = new HashSet<DbUser>();
            InverseManager = new HashSet<DbEmployee>();
        }

        public int EmployeeId { get; set; }
        public int? ManagerId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Mail { get; set; }
        public string Login { get; set; }
        public Guid EmployeeGuid { get; set; }
        public bool Obsolete { get; set; }

        public virtual DbEmployee Manager { get; set; }
        public virtual ICollection<DbUser> Users { get; set; }
        public virtual ICollection<DbEmployee> InverseManager { get; set; }
    }
}
