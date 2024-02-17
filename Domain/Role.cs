using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Role
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("type")]
        public string Type { get; set; }
    }
}
