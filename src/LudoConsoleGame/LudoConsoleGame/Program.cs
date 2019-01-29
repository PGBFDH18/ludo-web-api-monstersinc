using System;
using System.Collections.Generic;


namespace LudoConsoleGame
{
    class Program
    {
        private static LudoGameAPIProcessor ludo = new LudoGameAPIProcessor();
        private static int gameId;
        private static bool IsRunning;
        static void Main(string[] args)
        {
            IsRunning = true;
            while (IsRunning)
            {
                bool IsNOtINgame = true; // To get a specific game started

                while (IsNOtINgame)
                {
                    MainScreen();
                    if (IsRunning == false) // To exit out of program
                        break;
                    Console.Write("choose Game Id: ");
                    int.TryParse(Console.ReadLine(), out gameId);
                    if (ludo.GameById(gameId) == null)
                        Console.WriteLine("Invalid Game ID");
                    else
                        IsNOtINgame = false;
                }
                if (IsRunning == false) // to exit out of program
                    break;
                AddPlayers();
                PlayLudo();
            }
        }

        private static void MainScreen()
        {
            string input = "";
            bool IsMainScreen = true;
            do
            {
                Console.WriteLine("Welcome to Ludo Game");
                Console.WriteLine("[1]Creat New Game");
                Console.WriteLine("[2]Show List of Active Games");
                Console.WriteLine("[3]Play");
                Console.WriteLine("[4]Delete A game");
                Console.WriteLine("[5]Exit programm");
                Console.Write("Choice: ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ludo.CreateNewGame();
                        break;
                    case "2":

                        Dictionary<int, LudoGame> activeGames = ludo.ActiveGames();
                        Console.WriteLine("Current Active Game:");

                        try
                        {
                            foreach (KeyValuePair<int, LudoGame> entry in activeGames)
                                Console.WriteLine("Ludo Game, ID: " + entry.Key);
                        }
                        catch
                        {
                            Console.WriteLine("No active games available");
                        }
                        break;
                    case "3":
                        IsMainScreen = false;
                        break;
                    case "4":
                        ludo.DeleteGame(gameId);
                        break;
                    case "5":
                        IsRunning = false;
                        return;                        
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }

            } while (IsMainScreen);
        }

        private static void PlayLudo()
        {
            Console.WriteLine("Press Any key to start game");
            Console.ReadKey();
            Console.WriteLine("Game " + ludo.StartGame(gameId));
            Console.WriteLine("Press Any key to roll the dice");
            Console.WriteLine(ludo.RollDiece(gameId));

            do
            {
                var currentPlayer = ludo.GetCurrentPlayer(gameId);
                Console.WriteLine($"Current player is {currentPlayer.Name}");
                var dieceResult = ludo.RollDiece(gameId);
                Console.WriteLine($"Diece roll gave: {dieceResult}");
                Console.WriteLine("Choose which piece to move:");
                foreach (var piece in currentPlayer.Pieces)
                {
                    if (piece.State == "3")
                        continue;
                    Console.WriteLine($"{piece.PieceId}. located in positin {piece.Position}");
                }
                var pieceNr = Console.ReadLine();
                int.TryParse(pieceNr, out int pieceIdToMove);
                ludo.MovePiece(1, pieceIdToMove, Convert.ToInt32(dieceResult));
                Console.WriteLine(ludo.EndTurn(gameId, ludo.GetCurrentPlayer(gameId).PlayerId));
            } while (ludo.GetWinner(gameId) == null);

            Console.WriteLine($"{ludo.GetWinner(gameId).Name} is the winner!!!");

            Console.Write("Press any key to exit!");
            Console.ReadKey();
        }

        private static void AddPlayers()
        {
            do
            {
                Console.WriteLine("Choose color:");
                Console.WriteLine("0. Red");
                Console.WriteLine("1. Green");
                Console.WriteLine("2. Blue");
                Console.WriteLine("3. Yellow");
                Console.Write("Enter number (press enter to skip) :");
                var color = Console.ReadLine();
                if (string.IsNullOrEmpty(color))
                {
                    return;
                }
                int.TryParse(color, out int colorNumber);

                Console.Write("Enter playername :");
                var playername = Console.ReadLine();

                string Response = ludo.AddNewPalyer(gameId, playername, Convert.ToString(colorNumber));

                Console.WriteLine(Response);
            } while (ludo.NumberOfPlayersAdded(gameId) < 2);
        }

    }
}