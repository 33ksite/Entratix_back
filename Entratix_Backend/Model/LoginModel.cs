using System.ComponentModel.DataAnnotations;
namespace Entratix_Backend.Model
{
    public class LoginModel
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
