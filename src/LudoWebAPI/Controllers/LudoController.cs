using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LudoGameEngine;

namespace LudoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LudoController : ControllerBase
    {
        // GET api/ludo
        [HttpGet]
        public Dictionary<int, LudoGame> GetGames()
        {
            return Game.activeGames;
        }
        // POST api/ludo
        [HttpPost]
        public OkResult NewGame()
        {
            int id = Game.CreateNewGame();
            return Ok();
        }
        // GET api/ludo/2
        [HttpGet("{gameId}")]
        public LudoGame GetGame(int gameId)
        {
            return Game.activeGames[gameId];
        }
        // DELETE api/ludo/2
        [HttpDelete("{gameId}")]
        public OkResult Delete(int gameId)
        {
            Game.activeGames.Remove(gameId);
            return Ok();
        }

        // GET api/ludo/2/player
        [HttpGet("{gameId}/players")]
        public Player[] GetPlayers(int gameId)
        {
            return Game.activeGames[gameId].GetPlayers();
        }

        // POST api/ludo/2/player?name=Brad&color=red
        [HttpPost("{gameId}/player")]
        public OkResult AddPlayer(int gameId, string name, PlayerColor color)
        {
            Game.activeGames[gameId].AddPlayer(name, color);
            return Ok();
        }

        // GET api/ludo/2/player/2
        [HttpGet("{gameId}/player/{playerId}")]
        public Player GetPlayer(int gameId, int playerId)
        {
            return Game.activeGames[gameId]._players[playerId];
        }
    }
}
