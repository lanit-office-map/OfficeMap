using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class Employee
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
    }
}
