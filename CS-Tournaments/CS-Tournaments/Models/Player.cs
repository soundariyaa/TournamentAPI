using System.ComponentModel.DataAnnotations;

namespace CS_Tournaments.Api.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? PlayerName { get; set; }

        [Required]
        public int Age { get; set; }

        // Navigation property for many-to-many relationship
        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}