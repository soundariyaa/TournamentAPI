namespace CS_Tournaments.Api.Models
{
    public class ResponseTournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentTournamentId { get; set; }

        public List<ResponseSubTournament> SubTournaments { get; set; } = new();

        public List<ResponsePlayer> Players { get; set; } = new();

    }
}
