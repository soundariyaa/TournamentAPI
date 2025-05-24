using CS_Tournaments.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace CS_Tournaments.Api.Models
{
    public class PlayerResponse
    {
        public int Id { get; set; }

        public string? PlayerName { get; set; }

        public int Age { get; set; }

    }
}
