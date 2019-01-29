using LudoGameEngine;
using LudoWebAPI.Controllers;
using LudoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;

namespace LudoAPITest
{
    public class UnitTests
    {
        IGameContainer _gameContainer;
        LudoController _ludoController;


        public UnitTests()
        {
            _gameContainer = new FakeGameContainer();
            _ludoController = new LudoController(_gameContainer);
        }

        // Helper Method
        private void EmptyGameList()
        {
            _gameContainer.Gamesloader().Clear();
        } 

        [Fact]
        public void GetGames_GameListISEmpty_ReturnsNotFoundResult()
        {
            // Arrange
            EmptyGameList();

            // Act
            var notFoundResult = _ludoController.GetGames();

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);

        }

        [Fact]
        public void GetGames_GameListIsNotEmpty_ReturnsOKdResult()
        {
            // Act
            var okResult = _ludoController.GetGames();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);            
        }

        [Fact]
        public void GetGames_GameListIsNotEmpty_ReturnsListofActiveGames()
        {
            // Act
            var okResult = _ludoController.GetGames().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<Dictionary<int, ILudoGame>>(okResult.Value);
            Assert.Equal(7, items.Count);
        }

        [Fact]
        public void NewGame_AddNewGame_ReturnsOKdResult()
        {
            // Act
            var okResult = _ludoController.NewGame();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }


        // Gets a specific Active game by Id
        [Fact]
        public void GetGame_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGame(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetGame_ValidGameId_RetrunsOkresult()
        {
            // Arrange
            int gameId = 1;

            // Act
            var okResult = _ludoController.GetGame(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

        }

        [Fact]
        public void GetGame_ValidGameId_ReturnsActiveGame()
        {
            // Arrange
            int gameId = 1;

            // Act
            var OkResult = _ludoController.GetGame(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<LudoGame>(OkResult.Value);
        }

        [Fact]
        public void StartGame_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void StartGame_GameAlreadyOnorEnded_ReturnsUnauthorized()
        {
            // Arrange
            int gameId = 2; // Check FakeGameConiter for HardCoded test games           

            // Act
            var unauthorized = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized.Result);
        }

        [Fact]
        public void StartGame_WrongNumberOfPlayers_ReturnsBadRequest()
        {
            // Arrange
            int gameId = 4; // Check FakeGameConiter for HardCoded test games           

            // Act
            var badRequest = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequest.Result);
        }

        [Fact]
        public void StartGame_NotstartedStateAndrightNumberOfPLayers_RetrunsOkResult()
        {
            // Arrange
            int gameId = 3; // Check FakeGameConiter for HardCoded test games...Players between 2-4           

            // Act
            var okResult = _ludoController.StartGame(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void StartGame_ValidGameReult_ReturnsOkResult()
        {
            // Arrange
            int gameId = 1;

            // Act
            var okResult = _ludoController.Delete(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Delete_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.Delete(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void RollDiece_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.RollDiece(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void RollDiece_GameNotStartedOrEnded_ReturnsUnauthorized()
        {
            // Arrange
            int gameId = 1; // Check FakeGameConiter for HardCoded test games           

            // Act
            var unauthorized = _ludoController.RollDiece(gameId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized.Result);
        }

        [Fact]
        public void RollDiece_ValidGameId_RetrunsOkresult()
        {
            // Arrange
            int gameId = 2;

            // Act
            var okResult = _ludoController.RollDiece(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

        }

        [Fact]
        public void RollDiece_ValidGameId_RetrunsInteger()
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
        public void GetGameState_ValidGameId_RetrunsOkresult()
        {
            // Arrange
            int gameId = 2;

            // Act
            var okResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

        }

        [Fact]
        public void GetGameState_ValidGameId_RetrunsState()
        {
            // Arrange
            int gameId = 2;

            // Act
            var okResult = _ludoController.GetGameState(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<string>(okResult.Value);
        }

        [Fact]
        public void MovePiec_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void MovePiec_InvalidPiceId_RetrunsUnauthorizedResult()
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
        public void MovePiec_GameEnded_RetrunsUnauthorizedResult()
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
        public void MovePiec_GameNotStarted_RetrunsUnauthorizedResult()
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
        public void MovePiec_PieceInGoal_RetrunsUnauthorizedResult()
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
        public void MovePiec_OnePlayeGotAllPiecesInGoal_ReturnsOkResultAndAplayerName()
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
        public void MovePiec_PieceMovedByIdSuccesfully_RetrunsOkresult()
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
        public void GetAllPieces_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetAllPieces_ValidGameId_ReturnsOkResult()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetAllPieces(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetAllPieces_ValidGameId_ListOfAllPieces()
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
        public void GetPlayers_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetPlayers_ValidGameId_ReturnsOkResult()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetPlayers(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }


        [Fact]
        public void GetPlayers_ValidGameId_ListOfAllPlayers()
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
        public void AddPlayer_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetGameState(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void AddPlayer_GameStartedOrEnded_ReturnsUnauthorized()
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
        public void AddPlayer_ColorAlreadyInUse_ReturnsUnauthorized()
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
        public void AddPlayer_ValidGameIdAndColor_ReturnsOkResult()
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
        public void GetCurrentPlayer_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetCurrentPlayer(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetCurrentPlayer_GameNotStarted_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 3; //Not started game

            // Act
            var notFoundResult = _ludoController.GetCurrentPlayer(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetcurrentPlayer_ValidGameId_ReturnsOkResult()
        {
            // Arrange
            int gameId = 2; 
            
            // Act
            var okResult = _ludoController.GetCurrentPlayer(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetcurrentPlayer_ValidGameId_ReturnsAplayer()
        {
            // Arrange
            int gameId = 6;

            // Act
            var okResult = _ludoController.GetCurrentPlayer(gameId).Result as OkObjectResult ;

            // Assert
            Assert.IsType<Player>(okResult.Value);
        }

        [Fact]
        public void EndTurn_NotValidGameId_ReturnsNotFoundResult()
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
        public void EndTurn_PlayerIdNotMatchingCurrentPlayerId_ReturnsUnauthorized()
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
        public void EndTurn_ValidGameIdAndPlayerId_ReturnsOkResult()
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
        public void GetTheWinner_NotValidGameId_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 200;

            // Act
            var notFoundResult = _ludoController.GetWinner(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetTheWinner_NoWinnerFound_ReturnsNotFoundResult()
        {
            // Arrange
            int gameId = 6;

            // Act
            var notFoundResult = _ludoController.GetWinner(gameId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetTheWinner_WinnerPlayerFound_ReturnsOkResult()
        {
            // Arrange
            int gameId = 7;

            // Act
            var okResult = _ludoController.GetWinner(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTheWinner_WinnerPlayerFound_ReturnsWinner()
        {
            // Arrange
            int gameId = 7;

            // Act
            var okResult = _ludoController.GetWinner(gameId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Player>(okResult.Value);
        }

        [Fact]
        public void GetPlayer_NotValidGameId_ReturnsNotFoundResult()
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
        public void GetPlayer_NoPlayersAddedYet_ReturnsNotFoundResult()
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
        public void GetPlayer_NotValidPlayerId_ReturnsNotFoundResult()
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
        public void GetPlayer_GameIdAndPlayerIdAreValid_ReturnsOkResult()
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
        public void GetPlayer_GameIdAndPlayerIdAreValid_ReturnsTheSlectedPlayer()
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
