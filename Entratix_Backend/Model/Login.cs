using System.ComponentModel.DataAnnotations;
namespace Entratix_Backend.Model
{
    public class Login
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
