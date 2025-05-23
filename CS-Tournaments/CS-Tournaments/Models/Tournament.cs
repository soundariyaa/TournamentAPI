using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.Json.Serialization;

namespace CS_Tournaments.Api.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public int? ParentTournamentId { get; set; }
        public Tournament? ParentTournament { get; set; }

        public List<Tournament> SubTournaments { get; set; } = new();
        public List<Player> Players { get; set; } = new();
        //[Key]
        //public int Id { get; set; } // Primary Key

        //[Required]
        //public string? Name { get; set; } = string.Empty; // Tournament Name

        //public List<Player> Players { get; set; } = new(); // Registered Players

        ////[JsonIgnore] // Prevents serialization cycles
        //public List<Tournament> SubTournaments { get; set; } = new(); // Nested Sub-Tournaments


        //public int? ParentTournamentId { get; set; } // Nullable Parent Tournament ID

        //[ForeignKey("ParentTournamentId")]
        //[JsonIgnore]
        //public Tournament? ParentTournament { get; set; } // Navigation Property for Parent Tournament

    }
}
