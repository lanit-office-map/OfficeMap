using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class User
    {
        public Employee Employee { get; set; }
    }
}