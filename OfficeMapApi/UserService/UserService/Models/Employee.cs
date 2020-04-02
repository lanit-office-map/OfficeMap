﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int? ManagerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        public string Mail { get; set; }
        [Required]
        public string Login { get; set; }
        public virtual ICollection<Employee> InverseManager { get; set; }
    }
}
