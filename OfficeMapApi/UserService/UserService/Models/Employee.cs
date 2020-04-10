using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class Employee
    {
        public Guid EmployeeGuid { get; set; }
        public int? ManagerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        public string Mail { get; set; }
    }
}
