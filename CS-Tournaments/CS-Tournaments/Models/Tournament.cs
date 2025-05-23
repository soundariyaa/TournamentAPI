using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.Json.Serialization;

namespace CS_Tournaments.Api.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? ParentTournamentId { get; set; }
        public Tournament? ParentTournament { get; set; }

        public List<Tournament> SubTournaments { get; set; } = new();
        public ICollection<Player> Players { get; set; } = new List<Player>();

    }
}
