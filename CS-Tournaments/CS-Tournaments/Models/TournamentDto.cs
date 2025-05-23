namespace CS_Tournaments.Api.Models
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<TournamentDto>? SubTournaments { get; set; }
    }
}
