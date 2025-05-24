using CS_Tournaments.DBContext;
using CS_Tournaments.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CS_Tournaments.Models;
using AutoMapper;


namespace CS_Tournaments.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly TournamentDBContext _tournamentDBContext;
        private readonly IMapper _mapper;

        public TournamentsController(TournamentDBContext tournamentDBContext, IMapper mapper)
        {
            _tournamentDBContext = tournamentDBContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseTournament>>> GetTournament()
        {
            try
            {
                var tournaments = await _tournamentDBContext.Tournaments
                    .Include(t => t.SubTournaments)
                    .Include(t => t.Players).ToListAsync();
                var responseTournament = _mapper.Map<List<ResponseTournament>>(tournaments);
                return Ok(responseTournament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateTournament(CreateTournament createTournament)
        {
            try
            {
                var tournament = new Tournament
                {
                    Name = createTournament.Name,
                    ParentTournamentId = createTournament.ParentTournamentId,
                    SubTournaments = createTournament.SubTournaments.Select(st => new Tournament
                    {
                        Name = st.Name,
                        ParentTournamentId = st.ParentTournamentId
                    }).ToList(),
                    Players = createTournament.Players.Select(p => new Player
                    {
                        PlayerName = p.PlayerName,
                        Age = p.Age
                    }).ToList()
                };

                _tournamentDBContext.Tournaments.Add(tournament);
                await _tournamentDBContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating tournament: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseTournament>> GetTournamentById(int id)
        {
            try
            {
                var tournament = await _tournamentDBContext.Tournaments
          .Include(t => t.SubTournaments)
            .ThenInclude(t => t.SubTournaments)
          .Include(t => t.Players)
          .FirstOrDefaultAsync(t => t.Id == id);

                if (tournament == null) return NotFound();
                var responseTournament = _mapper.Map<ResponseTournament>(tournament);

                return Ok(responseTournament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving tournament for {id}: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(int id, [FromBody] CreateTournament updateTournament)
        {
            try
            {
                var tournament = await _tournamentDBContext.Tournaments
                    .Include(t => t.SubTournaments)
                    .Include(t => t.Players)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tournament == null)
                    return NotFound("Tournament not found.");

                tournament.Name = updateTournament.Name;
                tournament.ParentTournamentId = updateTournament.ParentTournamentId;

                // Optional: handle updates to players and sub-tournaments

                await _tournamentDBContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating tournament: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            try
            {
                var tournament = await _tournamentDBContext.Tournaments.FindAsync(id);
                if (tournament == null)
                    return NotFound("Tournament not found.");

                _tournamentDBContext.Tournaments.Remove(tournament);
                await _tournamentDBContext.SaveChangesAsync();
                return Ok("Tournament deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting tournament: {ex.Message}");
            }
        }

        [HttpGet("{id}/subtournaments")]
        public async Task<IActionResult> GetSubTournamentsById(int id)
        {
            try
            {
                var tournament = await _tournamentDBContext.Tournaments
                    .Include(t => t.SubTournaments)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tournament == null)
                    return NotFound("Tournament not found.");

                var subTournaments = _mapper.Map<List<ResponseSubTournament>>(tournament.SubTournaments);
                return Ok(subTournaments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching subtournaments: {ex.Message}");
            }
        }

        [HttpGet("{id}/players")]
        public async Task<IActionResult> GetPlayersByTournament(int id)
        {
            try
            {
                var tournament = await _tournamentDBContext.Tournaments
                    .Include(t => t.Players)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tournament == null)
                    return NotFound("Tournament not found.");

                var responsePlayers = _mapper.Map<List<ResponsePlayer>>(tournament.Players);
                return Ok(responsePlayers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching players: {ex.Message}");
            }
        }

        // Register a player in a tournament with hierarchy validation
        [HttpPost("{tournamentId}/register")]
        public async Task<IActionResult> RegisterPlayer(int tournamentId, [FromQuery] int playerId)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error registering player for the tournament {tournamentId}: {ex.Message}");
            }
        }
    }
}
