using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LudoGameEngine;

namespace LudoWebAPI.Models
{
    public class Sql
    {
        private static Dictionary<int, ILudoGame> ActiveGames;
        public static Dictionary<int, ILudoGame> activeGames
        {
            get
            {
                if (ActiveGames == null)
                {
                    ActiveGames = GetGamesFromServer();
                }

                return ActiveGames;
            }
        }

        static SqlConnection con = new SqlConnection("Data Source=JOCKES;Initial Catalog=Ludo;Integrated Security=True");

        private static Dictionary<int, ILudoGame> GetGamesFromServer()
        {
            var list = new Dictionary<int, ILudoGame> { };

            string command = 
                "SELECT Game.ID AS GameID, GameState, CurrentPlayerID, Player.LocalID AS PlayerID, Player.Name, " +
                "Player.Color, Player.Offset, Piece.LocalID AS PieceID, Piece.State AS PieceState, Piece.Position " +
                "FROM Game " +
                "JOIN Player ON Game.ID = Player.GameID " +
                "JOIN Piece ON Player.ID = Piece.PlayerID";
            var cmd = new SqlCommand(command, con);

            con.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = (int)reader["GameID"];
                
                if (!list.ContainsKey(id))
                {
                    list.Add(id, new LudoGame(new Diece()));
                    list[id].SetGameState((int)reader["GameState"]);
                    list[id].SetCurrentPlayer((int)reader["CurrentPlayerID"]);
                }

                if (list[id].GetPlayers().Length == 0)
                    list[id].AddPlayer((string)reader["Name"], (PlayerColor)reader["Color"]);
                else
                {
                    bool playerExist = false;

                    foreach (var p in list[id].GetPlayers())
                    {
                        if (p.PlayerId == (int)reader["PlayerID"])
                            playerExist = true;
                        break;
                    }

                    if (!playerExist)
                        list[id].AddPlayer((string)reader["Name"], (PlayerColor)reader["Color"]);
                }



                foreach (var p in list[id].GetPlayers())
                {
                    if (p.PlayerId == (int)reader["PlayerID"])
                    {
                        foreach (var pi in p.Pieces)
                        {
                            if (pi.PieceId == (int)reader["PieceID"])
                            {
                                pi.Position = (int)reader["Position"];
                                pi.State = (PieceGameState)(int)reader["PieceState"];
                            }
                        }
                    }
                }
            }
            con.Close();

            return list;
        }

        public static int CreateNewGame(LudoGame game)
        {
            var cmd = new SqlCommand(
                "INSERT INTO Game " +
                "VALUES(default, default) " +
                "SELECT TOP(1) ID FROM Game " +
                "ORDER BY ID DESC", con);

            con.Open();
            int id = (int)cmd.ExecuteScalar();
            con.Close();

            return id;
        }

        public static 
    }
}
