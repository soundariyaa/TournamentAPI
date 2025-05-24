using CS_Tournaments.Api.Models;
using CS_Tournaments.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace CS_Tournaments.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayersController : ControllerBase
    {
        private readonly TournamentDBContext _tournamentDBContext;

        public PlayersController(TournamentDBContext context) => _tournamentDBContext = context;

        [HttpGet]
        public async Task<ActionResult> GetPlayers()
        {
            var players = await _tournamentDBContext.Players.ToListAsync();
            if (players == null || !players.Any())
            {
                return NotFound("No Players Found");
            }
            List<PlayerResponse> response = new List<PlayerResponse>();
            response = players.Select(p => new PlayerResponse
            {
                Id = p.Id,
                PlayerName = p.PlayerName,
                Age = p.Age
            }).ToList();               
            return Ok(response);
        }

            [HttpPost]
        public async Task<ActionResult> CreatePlayer(Player player)
        {
            _tournamentDBContext.Players.Add(player);
           await _tournamentDBContext.SaveChangesAsync();
           
            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _tournamentDBContext.Players.FirstOrDefaultAsync(p => p.Id == id);
            return player == null ? NotFound() : Ok(player);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _tournamentDBContext.Players.FindAsync(id);
            if (player == null) return NotFound();
            _tournamentDBContext.Players.Remove(player);
            await _tournamentDBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
