namespace CS_Tournaments.Api.Models
{
    public class ResponseSubTournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentTournamentId { get; set; }
        public List<ResponseSubTournament> SubTournaments { get; set; } = new();
    }
}