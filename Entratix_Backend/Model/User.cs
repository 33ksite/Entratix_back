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

        public string Token { get; set; }

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
