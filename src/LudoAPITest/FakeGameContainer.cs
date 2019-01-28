using LudoGameEngine;
using LudoWebAPI.Models;
using System.Collections.Generic;

namespace LudoAPITest
{
    public class FakeGameContainer : IGameContainer
    {
        private readonly Dictionary<int, ILudoGame> _activeGames;
        private readonly ILudoGame _ludoGame;
        private readonly IDiece _diece;


        public FakeGameContainer()
        {

            _diece = new Diece();
            _ludoGame = new LudoGame(_diece);
            Piece[] testPiece = new Piece[]
            {
              new Piece() {PieceId = 0, Position = 53, State = PieceGameState.GoalPath},
              new Piece() {PieceId = 1, Position = 31, State = PieceGameState.Goal},
              new Piece() {PieceId = 2, Position = 51, State = PieceGameState.Goal},
              new Piece() {PieceId = 3, Position = 52, State = PieceGameState.Goal},
            };
            Piece[] testPiece2 = new Piece[]
            {
              new Piece() {PieceId = 0, Position = 30, State = PieceGameState.InGame},
              new Piece() {PieceId = 1, Position = 31, State = PieceGameState.InGame},
              new Piece() {PieceId = 2, Position = 51, State = PieceGameState.GoalPath},
              new Piece() {PieceId = 3, Position = 52, State = PieceGameState.GoalPath},
            };
            Piece[] testPiece3 = new Piece[] // All in Goal
            {
              new Piece() {PieceId = 0, Position = 56, State = PieceGameState.Goal},
              new Piece() {PieceId = 1, Position = 56, State = PieceGameState.Goal},
              new Piece() {PieceId = 2, Position = 56, State = PieceGameState.Goal},
              new Piece() {PieceId = 3, Position = 56, State = PieceGameState.Goal},
            };

            //A test List of Active Games with Different senarios
            _activeGames = new Dictionary<int, ILudoGame>()
            {
                {
                    1, new LudoGame(_diece)
                    { currentPlayerId = 0, _gameState = GameState.Ended, _players = new List<Player>() }
                },
                {
                    2, new LudoGame(_diece)
                    { currentPlayerId = 0, _gameState = GameState.Started, _players = new List<Player>() }
                },
                {
                    3, new LudoGame(_diece)
                    { currentPlayerId = 0, _gameState = GameState.NotStarted, _players = new List<Player>(){ new Player(), new Player() } }
                },
                {
                    4, new LudoGame(_diece)
                    { currentPlayerId = 0, _gameState = GameState.NotStarted, _players = new List<Player>(){new Player()} }
                },
                {
                    5, new LudoGame(_diece)
                    { currentPlayerId = 0, _gameState = GameState.Started, _players = new List<Player>(){ new Player() {PlayerId = 0, Pieces = testPiece, PlayerColor = PlayerColor.Red }, new Player() } }
                },
                {
                    6, new LudoGame(_diece)
                    { currentPlayerId = 0, _gameState = GameState.Started, _players = new List<Player>(){ new Player() {PlayerId = 0, Pieces = testPiece2 }, new Player() { PlayerId = 0, Pieces = testPiece2 } } }
                },
                {
                    7, new LudoGame(_diece)
                    { currentPlayerId = 0, _gameState = GameState.Started, _players = new List<Player>(){ new Player() {PlayerId = 0, Pieces = testPiece3 }, new Player() { PlayerId = 0, Pieces = testPiece2 } } }
                },
            };


        }

        public Dictionary<int, ILudoGame> Gamesloader()
        {

            return _activeGames;
        }


        public int AddNewGame()
        {
            int newId = 1;


            if (_activeGames.Count > 0)
            {
                foreach (var pair in _activeGames)
                {
                    if (pair.Key >= newId)
                        newId = pair.Key + 1;
                }
            }

            _activeGames.Add(newId, new LudoGame(_diece));
            return newId;
        }
    }
}
