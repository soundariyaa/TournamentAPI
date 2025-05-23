using System.ComponentModel.DataAnnotations;

namespace CS_Tournaments.Api.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string? PlayerName { get; set; }

        public int Age { get; set; }

        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}