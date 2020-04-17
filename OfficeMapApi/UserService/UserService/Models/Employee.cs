using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UserService.Models
{
    public class Employee
    {
        [IgnoreDataMember]
        public Guid EmployeeGuid { get; set; }
        public int? ManagerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
    }
}
