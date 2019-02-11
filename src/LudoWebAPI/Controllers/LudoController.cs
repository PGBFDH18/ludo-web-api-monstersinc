using LudoGameEngine;
using LudoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LudoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LudoController : ControllerBase
    {
        private readonly Dictionary<int, ILudoGame> _activeGames;
        private readonly IGameContainer _game;

        public LudoController(IGameContainer game)
        {
            _game = game;
            _activeGames = game.Gamesloader();
        }

        /// <summary>
        /// Returns all active games
        /// </summary>
        /// <returns>List of active games</returns>
        // GET api/ludo
        [HttpGet]
        [ProducesResponseType(typeof(Dictionary<int, object>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<Dictionary<int, ILudoGame>>> GetGames()
        {
            //checks for active games
            if (_activeGames.Count() == 0)
                return NotFound("No games are running at the moment");

            return Ok(_activeGames);
        }

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <returns>Confirmation message that a game is created</returns>
        /// <response code="200">New Game added</response>       
        // POST api/ludo
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> NewGame()
        {
            int id = _game.AddNewGame();
            return Ok(id);
        }

        /// <summary>
        /// Returns a specific game by Id
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>Game object</returns>
        /// <response code="200">OK</response>
        /// <response code="404">Game was not found</response>
        // GET api/ludo/2
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public ActionResult<LudoGame> GetGame(int gameId)
        {
            // checks game by id in the dictionary
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId not found");

            return Ok((LudoGame)_activeGames[gameId]);
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        /// <param name="gameId">Unique Identifier for a game</param>
        /// <returns>Game state</returns>
        /// /// <response code="200">Started</response>
        /// <response code="404">Game was not found</response>
        //  PUT api/ludo/2
        [HttpPut("{gameId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public ActionResult<bool> StartGame(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var gameState = _activeGames[gameId].GetGameState();

            if (gameState != GameState.NotStarted)
            {
                return Unauthorized("Unable to start game since it has the state " + gameState + ". Only NotStarted games can be started");
            }

            var players = _activeGames[gameId].GetPlayers();

            if (players.Count() < 2 || players.Count() > 4)
            {
                return BadRequest("Plyers must be more than 2 and maximum of 4");
            }

            _activeGames[gameId].StartGame();

            return Ok(_activeGames[gameId].GetGameState().ToString());
        }

        /// <summary>
        /// Deletes a game by Id
        /// </summary>
        /// <param name="gameId">Unique Identifier for a game</param>
        /// <returns>Confirmation with game Id</returns>
        /// <respose code="200">Game deleted</respose>
        /// <response code="404">Game was not found</response>         
        // DELETE api/ludo/2
        [HttpDelete("{gameId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public ActionResult<string> Delete(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            _activeGames.Remove(gameId);
            return Ok("Game " + gameId + " deleted.");
        }

        /// <summary>
        /// Return an integer from a diece roll bewteen 1 and 6
        /// </summary>
        /// <param name="gameId">Unique Identifier for a game</param>
        /// <returns>A number beteween 1 and 6 </returns>
        /// <response code="200">number returned</response>
        /// <response code="404">Game was not found</response>
        /// <response code="401">Game not started</response>
        // GET api/ludo/2/roll
        [HttpGet("{gameId}/roll")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public ActionResult<int> RollDiece(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var game = _activeGames[gameId];
            var state = game.GetGameState();

            if (state != GameState.Started)
            {
                return Unauthorized("Unable roll diece since the game is not started, it's current state is: " + state);
            }

            return Ok(game.RollDiece());
        }

        /// <summary>
        /// Returns whether game started, ended och not started 
        /// </summary>
        /// <param name="gameId">Unique Identifier for a game</param>
        /// <returns>Game state</returns>
        /// <response code="200">Game state</response>
        /// <response code="404">Game not found</response>
        // GET api/ludo/2/state
        [HttpGet("{gameId}/state")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        public ActionResult<string> GetGameState(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            return Ok(_activeGames[gameId].GetGameState().ToString());
        }

        /// <summary>
        /// Move the piece in turn
        /// </summary>
        /// <param name="gameId">Unique Identifier for a game</param>
        /// <param name="pieceId">Unique Identifier for a piece</param>
        /// <param name="roll">The result of a dice roll</param>
        /// <returns>Piece moved</returns>
        /// <response code="200">Pice Moved</response>
        /// <response code="200">A winner found</response>
        /// <response code="404">Game not found</response>
        /// <response code="401">Invalid PiceID, must be between 0 and 3</response>
        /// <response code="401">Game is not yet started</response>
        /// <response code="401">Game is ended</response>
        /// <response code="401">Pice is in goal</response>
        /// <response code="404">wrong Player</response>        
        // PUT api/ludo/2/movepiece?pieceId=1&roll=5
        [HttpPut("{gameId}/movepiece")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(401)]
        [ProducesResponseType(401)]
        [ProducesResponseType(401)]
        public ActionResult<string> MovePiece(int gameId, int pieceId, int roll)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            if (pieceId < 0 || pieceId > 3)
                return Unauthorized("Invalid PieceId, must be between 0 and 3");

            var game = _activeGames[gameId];
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

            player = _activeGames[gameId].GetWinner();

            if (player != null)
            {
                return Ok("The winner is " + player.Name);
            }

            return Ok("Piece moved");
        }

        /// <summary>
        /// Returns all pieces
        /// </summary>
        /// <param name="gameId">Unique Identifier for a game</param>
        /// <returns>List of pieces</returns>
        /// <response code="200">List of pieces {id, position, state}</response>
        /// <response code="404">Game not found</response>
        // GET api/ludo/2/allpieces
        [HttpGet("{gameId}/allpieces")]
        [ProducesResponseType(typeof(Array), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Piece[]> GetAllPieces(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var game = _activeGames[gameId];
            return Ok(game.GetAllPiecesInGame());
        }

        /// <summary>
        /// Returns a list of players in an active game by game id
        /// </summary>
        /// <param name="gameId">Unique game Identifier</param>
        /// <returns>List of players</returns>
        /// <response code="200">List of active players</response>
        /// <response code="404">Game not found, Wrong id</response>
        // GET api/ludo/2/player
        [HttpGet("{gameId}/player")]
        [ProducesResponseType(typeof(Array), 200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<Player[]>> GetPlayers(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            return Ok(_activeGames[gameId].GetPlayers());
        }

        /// <summary>
        /// Adds new players to an active game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns>Confirmation message</returns>
        /// <response code="200">New player added with name and color</response>
        /// <response code="404">New player added with name and color</response>
        /// <response code="401">Game started or ended</response>
        /// <response code="401">Color already in use</response>
        // POST api/ludo/2/player?name=Brad&color=red
        [HttpPost("{gameId}/player")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(401)]
        public ActionResult<string> AddPlayer(int gameId, string name, PlayerColor color)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var gameState = _activeGames[gameId].GetGameState();
            if (gameState != GameState.NotStarted)
            {
                return Unauthorized("Unable to add player since game is " + gameState);
            }

            var player = _activeGames[gameId].AddPlayer(name, color);
            if (player == null)
            {
                return Unauthorized("The color is already used by another player");
            }

            return Ok("New player added. Name: " + name + ", Color: " + color);
        }

        /// <summary>
        /// Returns the player in current turn
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>Active player</returns>
        /// <response code="200">Player {Id, color, All Pieces}</response>
        /// <response code="404">Game not found</response>
        /// <response code="404">Game not started</response>
        // GET api/ludo/2/player/current
        [HttpGet("{gameId}/player/current")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(404)]
        public ActionResult<Player> GetCurrentPlayer(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            if (_activeGames[gameId].GetGameState() == GameState.NotStarted)
            {
                return NotFound("The game has not started yet");
            }

            return Ok(_activeGames[gameId].GetCurrentPlayer());
        }

        /// <summary>
        /// Ends current player turn
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <returns>confirmation message</returns>
        /// <response code="200">Turn ended and check for winner done</response>
        /// <response code="404">Game not found</response>
        /// <response code="401">Wrong player</response>
        // PUT api/ludo/2/player/2/endturn
        [HttpPut("{gameId}/player/{playerId}/endturn")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public ActionResult<Player> EndTurn(int gameId, int playerId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var currentPlayer = _activeGames[gameId].GetCurrentPlayer();

            if (playerId != currentPlayer.PlayerId)
                return Unauthorized($"Wrong player, it's currently {currentPlayer.PlayerId}");

            _activeGames[gameId].EndTurn(currentPlayer);

            return Ok("Turn ended and check for a winner done");
        }

        /// <summary>
        /// Returns a winner palyer
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>The winner {id, color, all pieces}</returns>
        /// <response code="200">winner found</response>
        /// <response code="404">Game not found</response>
        /// <response code="404">winner not found</response>
        // GET api/ludo/2/winner
        [HttpGet("{gameId}/winner")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(404)]
        public ActionResult<Player> GetWinner(int gameId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var player = _activeGames[gameId].GetWinner();

            if (player == null)
            {
                return NotFound(null);
            }

            return Ok(player);
        }

        /// <summary>
        /// Retruns a player by id
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <returns>Player {id, color, pices}</returns>
        /// <response code="200">player info</response>
        /// <response code="404">Game not found</response>
        /// <response code="404">Player not found</response>
        /// <response code="404">Game not found</response>
        /// <response code="404">player not found</response>
     // GET api/ludo/2/player/2
        [HttpGet("{gameId}/player/{playerId}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(404)]
        public ActionResult<Player> GetPlayer(int gameId, int playerId)
        {
            if (!_activeGames.ContainsKey(gameId))
                return NotFound("gameId is not found");

            var players = _activeGames[gameId].GetPlayers();

            if (players.Count() == 0)
            {
                return NotFound("This game has no players yet");
            }

            if (playerId > (players.Count() - 1) || playerId < 0)
                return NotFound("Player not found");

            return Ok(players[playerId]);
        }
    }
}
