using System.ComponentModel.DataAnnotations;

namespace Entratix_Backend.Model
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        public string Phone { get; set; }

        public UserModel CreateUserModel()
        {
           
            UserModel userModel = new UserModel
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Phone = this.Phone ?? "", 
                PasswordSalt = new byte[0],
                PasswordHash = new byte[0],
                Token = "",
                TokenCreated = DateTime.MinValue,
                TokenExpires = DateTime.MinValue
            };

            return userModel;
        }


    }
}

