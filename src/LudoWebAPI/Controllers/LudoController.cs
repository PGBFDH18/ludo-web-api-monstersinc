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
            if (Game.activeGames.Count() == 0)
                return NotFound("No games are running at the moment");

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
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId not found");

            return Ok((LudoGame)Game.activeGames[gameId]);
        }

        //  PUT api/ludo/2
        [HttpPut("{gameId}")]
        public ActionResult<bool> StartGame(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var gameState = Game.activeGames[gameId].GetGameState();

            if (gameState != GameState.NotStarted)
            {
                return Unauthorized("Unable to start game since it has the state " + gameState + ". Only NotStarted games can be started");
            }

            var players = Game.activeGames[gameId].GetPlayers();

            if (players.Count() < 2 || players.Count() > 4)
            {
                return BadRequest("Plyers must be more than 2 and maximum of 4");
            }           

            return Ok(Game.activeGames[gameId].StartGame());
        }

        // DELETE api/ludo/2
        [HttpDelete("{gameId}")]
        public ActionResult<string> Delete(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            Game.activeGames.Remove(gameId);
            return Ok("Game " + gameId + " deleted.");
        }

        // GET api/ludo/2/roll
        [HttpGet("{gameId}/roll")]
        public ActionResult<int> RollDiece(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var game = Game.activeGames[gameId];
            var state = game.GetGameState();

            if (state != GameState.Started)
            {
                return Unauthorized("Unable roll diece since the game is not started, it's current state is: " + state);
            }

            return Ok(game.RollDiece());
        }

        // GET api/ludo/2/state
        [HttpGet("{gameId}/state")]
        public ActionResult<int> GetGameState(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            return Ok(Game.activeGames[gameId].GetGameState());
        }

        // PUT api/ludo/2/movepiece?pieceId=2&roll=4
        [HttpPut("{gameId}/movepiece")]
        public ActionResult<string> MovePiece(int gameId, int pieceId, int roll)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var game = Game.activeGames[gameId];
            var state = game.GetGameState();

            if (state == GameState.Ended)
            {
                return Unauthorized("Game is ended, and a winner is found");
            }

            if (state == GameState.NotStarted)
            {
                return Unauthorized("Game is not yet started, please start the game");
            }

            Player player = game.GetCurrentPlayer();
            var piece = player.Pieces.First(p => p.PieceId == pieceId);

            if (piece.State == PieceGameState.Goal)
            {
                return Unauthorized("Piece is in Goal and unable to move");
            }

            game.MovePiece(player, pieceId, roll);

            int currentPlayerId = game.GetCurrentPlayer().PlayerId;

            if (player.PlayerId != currentPlayerId)
            {
                return NotFound("Wrong player, it's currently " + currentPlayerId);
            }

            game.EndTurn(player);

            player = Game.activeGames[gameId].GetWinner();

            if (player != null)
            {
                return Ok("The winner is " + player.Name);
            }

            return Ok("Piece moved");
        }

        // GET api/ludo/2/allpieces
        [HttpGet("{gameId}/allpieces")]
        public ActionResult<Piece[]> GetAllPieces(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var game = Game.activeGames[gameId];
            return Ok(game.GetAllPiecesInGame());            
        }

        // GET api/ludo/2/player
        [HttpGet("{gameId}/player")]
        public ActionResult<IEnumerable<Player[]>> GetPlayers(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            return Ok(Game.activeGames[gameId].GetPlayers());
        }

        // POST api/ludo/2/player?name=Brad&color=red
        [HttpPost("{gameId}/player")]
        public ActionResult<string> AddPlayer(int gameId, string name, PlayerColor color)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var gameState = Game.activeGames[gameId].GetGameState();
            if (gameState != GameState.NotStarted)
            {
                return Unauthorized("Unable to add player since game is " + gameState);
            }

            var player = Game.activeGames[gameId].AddPlayer(name, color);
            if (player == null)
            {
                return Unauthorized("The color is already used by another player");
            }

            return Ok("New player added. Name: " + name + ", Color: " + color);
        }

        // GET api/ludo/2/player/current
        [HttpGet("{gameId}/player/current")]
        public ActionResult<Player> GetCurrentPlayer(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            if (Game.activeGames[gameId].GetGameState() == GameState.NotStarted)
            {
                return NotFound("The game has not started yet");
            }

            return Ok(Game.activeGames[gameId].GetCurrentPlayer());
        }

        // PUT api/ludo/2/player/2/endturn
        [HttpPut("{gameId}/player/{playerId}/endturn")]
        public ActionResult<Player> EndTurn(int gameId, int playerId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var currentPlayer = Game.activeGames[gameId].GetCurrentPlayer();

            if (playerId != currentPlayer.PlayerId)            
                return Unauthorized($"Wrong player, it's currently {currentPlayer.PlayerId}");

            Game.activeGames[gameId].EndTurn(currentPlayer);

            return Ok("Turn ended and check for a winner done");
        }

        // GET api/ludo/2/winner
        [HttpGet("{gameId}/winner")]
        public ActionResult<Player> GetWinner(int gameId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var player = Game.activeGames[gameId].GetWinner();

            if(player == null)
            {
                return NotFound("No winner found");
            }
            return Ok("Winner Found");
        }

        // GET api/ludo/2/player/2
        [HttpGet("{gameId}/player/{playerId}")]
        public ActionResult<Player> GetPlayer(int gameId, int playerId)
        {
            if (!Game.activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var players = Game.activeGames[gameId].GetPlayers();

            if (players.Count() == 0)
            {
                return NotFound("This game has no players yet");
            }

            if (playerId > (players.Count() -1) || playerId < 0)
                return NotFound("Player not found");

            return Ok(players[playerId]);
        }
    }
}
