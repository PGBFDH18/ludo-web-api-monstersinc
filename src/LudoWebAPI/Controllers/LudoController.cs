using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using LudoGameEngine;
using System.Collections;

namespace LudoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LudoController : ControllerBase
    {
        // GET api/ludo
        [HttpGet]
        public ActionResult<IEnumerable<Dictionary<int, ILudoGame>>> GetGames()
        {
            return Ok(Game.activeGames);
        }

        // POST api/ludo
        [HttpPost]
        public ActionResult<string> NewGame()
        {
            int id = Game.CreateNewGame();
            return Ok("New game started. Id: " + id);
        }

        // GET api/ludo/2
        [HttpGet("{gameId}")]
        public ActionResult<LudoGame> GetGame(int gameId)
        {
            return Ok((LudoGame)Game.activeGames[gameId]);
        }

        //  PUT api/ludo/2
        [HttpPut("{gameId}")]
        public ActionResult<bool> StartGame(int gameId)
        {
            if (Game.activeGames[gameId].GetPlayers().Length < 2)
            {
                return NotFound("Players cannot be less than 2.");
            }
            else if (Game.activeGames[gameId].GetPlayers().Length > 4)
            {
                return NotFound("Players cannot be more than 4.");
            }

            return Ok(Game.activeGames[gameId].StartGame());
        }

        // DELETE api/ludo/2
        [HttpDelete("{gameId}")]
        public ActionResult<string> Delete(int gameId)
        {
            Game.activeGames.Remove(gameId);
            return Ok("Game " + gameId + " deleted.");
        }

        // GET api/ludo/2/state
        [HttpGet("{gameId}/state")]
        public ActionResult<int> GetGameState(int gameId)
        {
            return Ok(Game.activeGames[gameId].GetGameState());
        }

        // GET api/ludo/2/player
        [HttpGet("{gameId}/player")]
        public ActionResult<IEnumerable<Player[]>> GetPlayers(int gameId)
        {
            return Ok(Game.activeGames[gameId].GetPlayers());
        }

        // POST api/ludo/2/player?name=Brad&color=red
        [HttpPost("{gameId}/player")]
        public ActionResult<string> AddPlayer(int gameId, string name, PlayerColor color)
        {
            Game.activeGames[gameId].AddPlayer(name, color);
            return Ok("New player added. Name: " + name + ", Color: " + color);
        }

        // GET api/ludo/2/player/2
        [HttpGet("{gameId}/player/{playerId}")]
        public ActionResult<Player> GetPlayer(int gameId, int playerId)
        {
            var game = (LudoGame)Game.activeGames[gameId];
            return Ok(game._players[playerId]);
        }
    }
}
