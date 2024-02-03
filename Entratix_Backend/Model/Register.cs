using System.ComponentModel.DataAnnotations;
namespace Entratix_Backend.Model
{
    public class Register
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
       
    }
}
