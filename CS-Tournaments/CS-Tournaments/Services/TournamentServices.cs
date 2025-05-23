using CS_Tournaments.Api.Models;
using CS_Tournaments.DBContext;

namespace CS_Tournaments.Services
{
    public class TournamentServices : ITournamentService
    {
        private readonly TournamentDBContext _tournamentDBContext;
        public TournamentServices( TournamentDBContext tournamentDBContext)
        {
           _tournamentDBContext = tournamentDBContext;
        }

        public async ValueTask<string> SaveTournament(Tournament tournament)
        {
            //_tournamentDBContext.Tournaments.Add(tournament);
            //await _tournamentDBContext.SaveChangesAsync();
            return "Saved Sucessfully";
        }
    }
}
