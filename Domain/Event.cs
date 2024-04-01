using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Event
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("userid")]
        public int UserId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("location")]
        public string Location { get; set; }
        [Column("cost")]
        public decimal Cost { get; set; }
        [Column("photo")]
        public string Photo { get; set; }
        public User User { get; set; }
    }
}
