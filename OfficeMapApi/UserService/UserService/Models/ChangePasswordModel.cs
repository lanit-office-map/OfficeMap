using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}
