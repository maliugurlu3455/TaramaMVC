using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set;}

        public string ReturnUrl { get; set; }
    }
}
