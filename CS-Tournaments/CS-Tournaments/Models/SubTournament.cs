namespace CS_Tournaments.Api.Models
{
    public class SubTournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentTournamentId { get; set; }
    }
}