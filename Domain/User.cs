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