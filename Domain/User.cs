using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("roleid")]
        public int RoleId { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("passwordsalt")]
        public byte[] PasswordSalt { get; set; }

        [Column("passwordhash")]
        public byte[] PasswordHash { get; set; }

        [Column("tokenexpires")]
        public DateTime? TokenExpires { get; set; } // Tipo de dato anulable

        [Column("tokencreated")]
        public DateTime? TokenCreated { get; set; } // Tipo de dato anulable

        [Column("token")]
        public string? Token { get; set; }

        public User()
        {
        }
        public List<Event> Events { get; set; }

        public User(int id, int roleId, string firstName, string lastName, string phone, string email)
        {
            Id = id;
            RoleId = roleId;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
        }
    }
}
