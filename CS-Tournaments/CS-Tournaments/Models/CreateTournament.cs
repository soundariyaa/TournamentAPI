namespace CS_Tournaments.Api.Models
{
    public class CreateTournament
    {
        public string Name { get; set; } = null!;
        public int? ParentTournamentId { get; set; }

        public List<CreateSubTournament> SubTournaments { get; set; } = new();

        public List<CreatePlayer> Players { get; set; } = new();
    }
}
