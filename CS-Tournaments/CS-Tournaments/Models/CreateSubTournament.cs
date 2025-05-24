namespace CS_Tournaments.Api.Models
{
    public class CreateSubTournament
    {
        public string Name { get; set; } = null!;
        public int? ParentTournamentId { get; set; }
    }
}