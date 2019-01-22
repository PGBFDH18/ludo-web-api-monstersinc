﻿using System;
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
            var gameState = Game.activeGames[gameId].GetGameState();

            if (gameState == GameState.NotStarted)
            {
                return NotFound("Unable to start game since it has the state " + gameState + ". Only NotStarted games can be started");
            }

            var players = Game.activeGames[gameId].GetPlayers();

            if (players.Count() < 2)
            {
                return NotFound("Atleast two players is needed to start the game");
            }

            if (players.Count() > 4)
            {
                return NotFound("A max of four players can be in the game");
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
            var gameState = Game.activeGames[gameId].GetGameState();
            if (gameState != GameState.NotStarted)
            {
                return NotFound("Unable to add player since game is " + gameState);
            }

            var player = Game.activeGames[gameId].AddPlayer(name, color);
            if (player == null)
            {
                return NotFound("The color is already used by another player");
            }

            return Ok("New player added. Name: " + name + ", Color: " + color);
        }

        // GET api/ludo/2/player/2
        [HttpGet("{gameId}/player/{playerId}")]
        public ActionResult<Player> GetPlayer(int gameId, int playerId)
        {
            var game = (LudoGame)Game.activeGames[gameId];

            if (game.GetPlayers().Count() == 0)
            {
                return NotFound("This game has no players yet");
            }

            return Ok(game._players[playerId]);
        }
    }
}
