using CS_Tournaments.DBContext;
using CS_Tournaments.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CS_Tournaments.Models;


namespace CS_Tournaments.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly TournamentDBContext _tournamentDBContext;

        public TournamentsController(TournamentDBContext tournamentDBContext)
        {
            _tournamentDBContext = tournamentDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournament()
       => await _tournamentDBContext.Tournaments.ToListAsync();


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Tournament>>> GetTournaments()
        //=> await _tournamentDBContext.Tournaments.Include(t => t.SubTournaments).ToListAsync();


        [HttpPost]
        public async Task<IActionResult> CreateTournament(Tournament tournament)
        {
            _tournamentDBContext.Tournaments.Add(tournament);
            await _tournamentDBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        }

        // Get tournament details including sub-tournaments and players

        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentById(int id)
        {
            var tournament = await _tournamentDBContext.Tournaments
          .Include(t => t.SubTournaments)
          .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null) return NotFound();

            return Ok(MapTournamentToDto(tournament));
            //var tournament = await _tournamentDBContext.Tournaments
            //    .Include(t => t.SubTournaments)
            //    .ThenInclude(st => st.SubTournaments.Take(5))
            //    .Include(t => t.Players)
            //    .FirstOrDefaultAsync(t => t.Id == id);
            //int depth = 5;
            //return tournament == null ? NotFound() : Ok(MapTournamentToDto(tournament,depth));
        }

        private TournamentDto MapTournamentToDto(Tournament tournament,int depth = 0 )
        {
            if (depth >= 5) // avoid going deeper than 5 levels
                return new TournamentDto { Id = tournament.Id, Name = tournament.Name };

            return new TournamentDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                SubTournaments = tournament.SubTournaments?.Select(st => MapTournamentToDto(st, depth + 1)).ToList()
            };
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateSubTournament(Tournament tournament)
        //{
        //    _tournamentDBContext.Tournaments.Add(tournament);
        //    await _tournamentDBContext.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        //}

        //[HttpPost]
        //public async Task<ActionResult> CreateTournament(TournamentRequest tournamentRequest)
        //{
        //    //if (await GetDepth(tournament.ParentTournamentId) >= 5)
        //    //    return BadRequest("Cannot nest beyond 5 levels");

        //    _tournamentDBContext.Tournaments.Add(tournamentRequest);

        //    return Ok(await _tournamentDBContext.SaveChangesAsync());
        //   // return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        //}




        //// Get the list of all tournaments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Tournament>>> GetAllTournaments()
        //{
        //    return await _tournamentDBContext.Tournaments.Include(t => t.SubTournaments).ToListAsync();
        //}


        //private async Task<int> GetDepth(int? tournamentId)
        //{
        //    int depth = 0;
        //    while (tournamentId != null)
        //    {
        //        var tournament = await _tournamentDBContext.Tournaments.FindAsync(tournamentId);
        //        if (tournament == null) break;
        //        depth++;
        //        tournamentId = tournament.ParentTournamentId;
        //    }
        //    return depth;
        //}

        //// Delete a tournament
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTournament(int id)
        //{
        //    var tournament = await _tournamentDBContext.Tournaments.FindAsync(id);
        //    if (tournament == null)
        //    {
        //        return NotFound();
        //    }

        //    _tournamentDBContext.Tournaments.Remove(tournament);
        //    await _tournamentDBContext.SaveChangesAsync();
        //    return NoContent();
        //}

        //// Register a player in a tournament with hierarchy validation
        [HttpPost("{tournamentId}/register")]
        public async Task<IActionResult> RegisterPlayer(int tournamentId, int playerId)
        {
            var tournament = await _tournamentDBContext.Tournaments.Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            var player = await _tournamentDBContext.Players.FindAsync(playerId);

            if (tournament == null || player == null)
                return NotFound("Tournament or Player not found.");

            // Check parent tournament constraint
            if (tournament.ParentTournamentId.HasValue)
            {
                var parentTournament = await _tournamentDBContext.Tournaments.Include(t => t.Players)
                    .FirstOrDefaultAsync(t => t.Id == tournament.ParentTournamentId.Value);

                if (parentTournament == null || !parentTournament.Players.Any(p => p.Id == playerId))
                    return BadRequest("Player must be registered in the parent tournament first.");
            }

            if (tournament.Players.Any(p => p.Id == playerId))
                return BadRequest("Player is already registered in this tournament.");

            tournament.Players.Add(player);
            await _tournamentDBContext.SaveChangesAsync();
            return Ok("Player registered successfully.");
        }

    }
}
