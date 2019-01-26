using LudoGameEngine;
using LudoWebAPI.Controllers;
using LudoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;

namespace LudoAPITest
{
    public class UnitTest1
    {
        IGameContainer _gameContainer;
        LudoController _ludoController;

        // Helper Method
        private void EmptyGameList()
        {
            _gameContainer.Gamesloader().Clear();
        } 

        public UnitTest1()
        {
            _gameContainer = new FakeGameContainer();
            _ludoController = new LudoController(_gameContainer);
        }

        [Fact]
        public void GetAllActiveGames_GameListISEmpty_ReturnsNotFoundResult()
        {
            // Arrange
            EmptyGameList();

            // Act
            var notFoundResult = _ludoController.GetGames();

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetAllActiveGames_GameListIsNotEmpty_ReturnsOKdResult()
        {
            // Act
            var okResult = _ludoController.GetGames();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);            
        }

        [Fact]
        public void GetAllActiveGames_GameListIsNotEmpty_ReturnsListofActiveGames()
        {
            // Act
            var okResult = _ludoController.GetGames().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<Dictionary<int, ILudoGame>>(okResult.Value);
            Assert.Equal(7, items.Count);
        }

        [Fact]
        public void CreateNewGame_AddNewGame_ReturnsOKdResult()
        {
            // Act
            var okResult = _ludoController.NewGame();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetAtiveGameById_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGame(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetActiveGameById_ValidGameId_RetrunsOkresult()
        {
            // Arrange
            int gameId = 1;

            // Act
            var okResult = _ludoController.GetGame(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

        }

        [Fact]
        public void GetActiveGameById_ValidGameId_ReturnsActiveGame()
        {
            // Arrange
            int gameId = 1;

            // Act
            var OkResult = _ludoController.GetGame(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<LudoGame>(OkResult.Value);
        }

        [Fact]
        public void StartGameById_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void StartGameById_GameAlreadyOnorEnded_ReturnsUnauthorized()
        {
            // Arrange
            int gameId = 2; // Check FakeGameConiter for HardCoded test games           

            // Act
            var unauthorized = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized.Result);
        }

        [Fact]
        public void StartGameById_WrongNumberOfPlayers_ReturnsBadRequest()
        {
            // Arrange
            int gameId = 4; // Check FakeGameConiter for HardCoded test games           

            // Act
            var badRequest = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequest.Result);
        }

        [Fact]
        public void StartGameById_NotstartedStateAndrightNumberOfPLayers_RetrunsOkResult()
        {
            // Arrange
            int gameId = 3; // Check FakeGameConiter for HardCoded test games...Players between 2-4           

            // Act
            var okResult = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void DeleteGameById_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.Delete(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void StartGameById_ValidGameReult_ReturnsOkResult()
        {
            // Arrange
            int gameId = 1;

            // Act
            var okResult = _ludoController.Delete(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetDiceRollResultForAnActiveGame_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.RollDiece(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetDiceRollResultForAnActiveGame_GameNotStartedOrEnded_ReturnsUnauthorized()
        {
            // Arrange
            int gameId = 1; // Check FakeGameConiter for HardCoded test games           

            // Act
            var unauthorized = _ludoController.RollDiece(gameId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized.Result);
        }

        [Fact]
        public void GetDiceRollResultForAnActiveGame_ValidGameId_RetrunsOkresult()
        {
            // Arrange
            int gameId = 2;

            // Act
            var okResult = _ludoController.RollDiece(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

        }

        [Fact]
        public void GetDiceRollResultForAnActiveGame_ValidGameId_RetrunsInteger()
        {
            // Arrange
            int gameId = 2;

            // Act
            var okResult = _ludoController.RollDiece(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<int>(okResult.Value);
            Assert.InRange((int)okResult.Value, 1, 7);

        }

        [Fact]
        public void GetAnActiveGameState_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetAnActiveGameState_ValidGameId_RetrunsOkresult()
        {
            // Arrange
            int gameId = 2;

            // Act
            var okResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

        }

        [Fact]
        public void GetAnActiveGameState_ValidGameId_RetrunsState()
        {
            // Arrange
            int gameId = 2;

            // Act
            var okResult = _ludoController.GetGameState(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<string>(okResult.Value);
        }

        [Fact]
        public void MoveAplayerPiec_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void MoveAplayerPiec_InvalidPiceId_RetrunsUnauthorizedResult()
        {
            // Arrange
            int gameId = 2;
            int pieceId = 5; //PieceId must be bewteen 0 and 3
            int roll = 5;

            // Act
            var unauthorizedResult = _ludoController.MovePiece(gameId, pieceId, roll);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
        }

        [Fact]
        public void MoveAplayerPiec_GameEnded_RetrunsUnauthorizedResult()
        {
            // Arrange
            int gameId = 1;
            int pieceId = 1; //PieceId must be bewteen 0 and 3
            int roll = 5;

            // Act
            var unauthorizedResult = _ludoController.MovePiece(gameId, pieceId, roll);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
        }

        [Fact]
        public void MoveAplayerPiec_GameNotStarted_RetrunsUnauthorizedResult()
        {
            // Arrange
            int gameId = 3;
            int pieceId = 2; 
            int roll = 5;

            // Act
            var unauthorizedResult = _ludoController.MovePiece(gameId, pieceId, roll);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
        }

        [Fact]
        public void MoveAplayerPiec_PieceInGoal_RetrunsUnauthorizedResult()
        {
            // Arrange
            int gameId = 5; //Game started and has right number of Players
            int pieceId = 3;// Piece is in goal -- Check FakeGameContainer for the entries
            int roll = 5;

            // Act
            var unauthorizedResult = _ludoController.MovePiece(gameId, pieceId, roll);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorizedResult.Result);
        }

        [Fact]
        public void MoveAplayerPiec_OnePlayeGotAllPiecesInGoal_ReturnsOkResultAndAplayerName()
        {
            // Arrange
            int gameId = 5;  // Game started and has right number of Players, All Pieces in Goal except one with id = 0
            int pieceId = 0; // Piece is in goalpath -- Check FakeGameContainer for the entries
            int roll = 5;    // Piece will be in Goal after this roll and a Check for winner will run

            // Act
            var okResult = _ludoController.MovePiece(gameId, pieceId, roll);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void MoveAplayerPiec_PieceMovedByIdSuccesfully_RetrunsOkresult()
        {
            // Arrange
            int gameId = 6;  // Game started and has right number of Players
            int pieceId = 0; // This pice is in game
            int roll = 5;    // The piece should be moved succesfully

            // Act
            var okResult = _ludoController.MovePiece(gameId, pieceId, roll);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

        }

        [Fact]
        public void GetAllPiecesInAGame_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetAllPiecesInAGame_ValidGameId_ReturnsOkResult()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetAllPieces(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }


        [Fact]
        public void GetAllPiecesInAGame_ValidGameId_ListOfAllPieces()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetAllPieces(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Piece[]>(okResult.Value);
            Assert.Equal(8, (okResult.Value as Piece[]).Length);
        }

        [Fact]
        public void GetAllPlayersInAGame_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetAllPlayersInAGame_ValidGameId_ReturnsOkResult()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetPlayers(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }


        [Fact]
        public void GetAllPlayersInAGame_ValidGameId_ListOfAllPlayers()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetPlayers(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Player[]>(okResult.Value);
            Assert.Equal(2, (okResult.Value as Player[]).Length);
        }

        [Fact]
        public void AddNewPlayer_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void AddNewPlayer_GameStartedOrEnded_ReturnsUnauthorized()
        {
            // Arrange
            int gameId = 1;
            string name = "John";
            PlayerColor color = PlayerColor.Blue;

            // Act
            var unauthorized = _ludoController.AddPlayer(gameId, name, color);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized.Result);
        }

        [Fact]
        public void AddNewPlayer_ColorAlreadyInUse_ReturnsUnauthorized()
        {
            // Arrange
            int gameId = 5;
            string name = "John";
            PlayerColor color = PlayerColor.Red;

            // Act
            var unauthorized = _ludoController.AddPlayer(gameId, name, color);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized.Result);
        }

        [Fact]
        public void AddNewPlayer_ValidGameIdAndColor_ReturnsOkResult()
        {
            // Arrange
            int gameId = 3; // Not started game
            string name = "John";
            PlayerColor color = PlayerColor.Blue;

            // Act
            var okResult = _ludoController.AddPlayer(gameId, name, color);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetCurrentPlayerByGameId_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetCurrentPlayer(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetCurrentPlayerByGameId_GameNotStarted_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 3; //Not started game

            // Act
            var notFoundResult = _ludoController.GetCurrentPlayer(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetcurrentPlayerByGameId_ValidGameId_ReturnsOkResult()
        {
            // Arrange
            int gameId = 2; 
            
            // Act
            var okResult = _ludoController.GetCurrentPlayer(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetcurrentPlayerByGameId_ValidGameId_ReturnsAplayer()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetCurrentPlayer(gameId).Result as OkObjectResult ;

            // Assert
            Assert.IsType<Player>(okResult.Value);
        }

        [Fact]
        public void EndTurnInAGame_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;
            int playerId = 1;

            // Act
            var notFoundResult = _ludoController.EndTurn(gameId, playerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void EndTurnInAGame_PlayerIdNotMatchingCurrentPlayerId_ReturnsUnauthorized()
        {
            // Arrange
            int gameId = 6;
            int playerId = 1;

            // Act
            var unauthorized = _ludoController.EndTurn(gameId, playerId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized.Result);
        }

        [Fact]
        public void EndTurnInAGame_ValidGameIdAndPlayerId_ReturnsOkResult()
        {
            // Arrange
            int gameId = 6;
            int playerId = 0; // matches current player

            // Act
            var okResult = _ludoController.EndTurn(gameId, playerId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTheWinnerPlayer_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetWinner(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetTheWinnerPlayer_NoWinnerFound_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 6;

            // Act
            var notFoundResult = _ludoController.GetWinner(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetTheWinnerPlayer_WinnerPlayerFound_ReturnsOkResult()
        {
            // Arrange
            int gameId = 7;

            // Act
            var okResult = _ludoController.GetWinner(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTheWinnerPlayer_WinnerPlayerFound_ReturnsWinner()
        {
            // Arrange
            int gameId = 7;

            // Act
            var okResult = _ludoController.GetWinner(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Player>(okResult.Value);
        }

        [Fact]
        public void GetPlayerById_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;
            int playerId = 0;

            // Act
            var notFoundResult = _ludoController.GetPlayer(gameId, playerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetPlayerById_NoPlayersAddedYet_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 2;
            int playerId = 0;

            // Act
            var notFoundResult = _ludoController.GetPlayer(gameId, playerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetPlayerById_NotValidPlayerId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 6;
            int playerId = 4;

            // Act
            var notFoundResult = _ludoController.GetPlayer(gameId, playerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetPlayerById_GameIdAndPlayerIdAreValid_ReturnsOkResult()
        {
            // Arrange
            int gameId = 6;
            int playerId = 0;

            // Act
            var okResult = _ludoController.GetPlayer(gameId, playerId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetPlayerById_GameIdAndPlayerIdAreValid_ReturnsTheSlectedPlayer()
        {
            // Arrange
            int gameId = 6;
            int playerId = 0;

            // Act
            var okResult = _ludoController.GetPlayer(gameId, playerId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Player>(okResult.Value);
        }

    }
}
