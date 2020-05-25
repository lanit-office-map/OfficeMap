using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class ChangeEmailModel
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }

        public bool RememberMe { get; set; }
    }
}
