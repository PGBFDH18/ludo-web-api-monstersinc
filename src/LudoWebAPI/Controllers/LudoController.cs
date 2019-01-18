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
        public ObjectResult NewGame()
        {
            int id = Game.CreateNewGame();
            return Ok("New game created. Game ID: " + id);
        }
        // GET api/ludo/2
        [HttpGet("{id}")]
        public LudoGame GetGame(int id)
        {
            return Game.activeGames[id];
        }
        // DELETE api/ludo/2
        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            Game.activeGames.Remove(id);
            return Ok("Game " + id + " deleted");
        }

        // GET api/ludo/2/player
        [HttpGet("{gameId}/player")]
        public List<Player> GetPlayers(int gameId)
        {
            return Game.activeGames[gameId]._players;
        }

        // POST api/ludo/2/player?brad&0
        [HttpPost("{gameId}/player")]
        public ObjectResult AddPlayer(int gameId, string name, PlayerColor color)
        {
            Game.activeGames[gameId].AddPlayer(name, color);
            return Ok(name + "(" + color + ") added to Game " + gameId);
        }

        // GET api/ludo/2/player/2
        [HttpGet("{gameId}/player/{playerId}")]
        public Player GetPlayer(int gameId, int playerId)
        {
            return Game.activeGames[gameId]._players[playerId];
        }
    }
}
