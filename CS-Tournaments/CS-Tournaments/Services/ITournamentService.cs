using CS_Tournaments.Api.Models;

namespace CS_Tournaments.Services
{
    public interface ITournamentService
    {
        ValueTask<string> SaveTournament(Tournament tournament);


    }
}
