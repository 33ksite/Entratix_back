using Domain;
using System.ComponentModel.DataAnnotations;
namespace Entratix_Backend.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }

        public string Token { get; set; }

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }


        public User CreateUserFromModel()
        {
            User user = new User
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Phone = this.Phone,
                PasswordSalt = this.PasswordSalt,
                PasswordHash = this.PasswordHash,
                Token = this.Token,
                TokenCreated = this.TokenCreated,
                TokenExpires = this.TokenExpires,
                RoleId = GetRoleId(this.Role)

            };

            return user;
        }

        public UserModel() { }

        public UserModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Role = GetRoleName(user.RoleId);
            Phone = user.Phone;
            PasswordSalt = user.PasswordSalt;
            PasswordHash = user.PasswordHash;
            Token = user.Token;
            TokenCreated = user.TokenCreated ?? DateTime.MinValue;
            TokenExpires = user.TokenExpires ?? DateTime.MinValue;
        }


        private string GetRoleName(int roleId)
        {

            switch (roleId)
            {

                case 1:
                    return "User";
                case 2:
                    return "RRPP";
                case 3:
                    return "Producer";
                case 4:
                    return "Administrator";
                default:
                    return "Unknown";
            }
        }

        private int GetRoleId(string roleName)
        {
            switch (roleName)
            {
                case "User":
                    return 1;
                case "RRPP":
                    return 2;
                case "Producer":
                    return 3;
                case "Administrator":
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
