using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS_Tournaments.Api.Models
{
    public class SubTournament
    {
        [Key]
        public int Id { get; set; } // Primary Key
        [ForeignKey("ParentTournamentId")]
        public int? ParentTournamentId { get; set; }
        public string? SubTournamentName { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
