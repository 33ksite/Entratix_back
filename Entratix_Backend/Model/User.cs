using System.ComponentModel.DataAnnotations;
namespace Entratix_Backend.Model
{
    public class User
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
    }
}
