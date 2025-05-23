namespace CS_Tournaments.Models
{
    public class TournamentRequest
    {
        public string tournamentName { get; set; } = null;
        public int numberOfPlayers { get; set; }
        public int parentTournamentId { get; set; }
    }
}
