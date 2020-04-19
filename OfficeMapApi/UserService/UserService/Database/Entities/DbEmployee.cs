using System;

namespace UserService.Database.Entities
{
    public partial class DbEmployee
    {
        public int EmployeeId { get; set; }
        public int? ManagerId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Mail { get; set; }
        public string Login { get; set; }
        public Guid EmployeeGuid { get; set; }
        public bool Obsolete { get; set; }

        public virtual DbUser User { get; set; }
    }
}
