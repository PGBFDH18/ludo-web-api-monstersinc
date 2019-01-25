using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGameEngine;

namespace LudoWebAPI.Models
{
    public class Game
    {
        //public static Dictionary<int, ILudoGame> activeGames = new Dictionary<int, ILudoGame> { };
        public static Dictionary<int, ILudoGame> activeGames = Sql.activeGames;

        public static int CreateNewGame()
        {
            // SQL version
            var game = new LudoGame(new Diece());
            int id = Sql.CreateNewGame(game);
            activeGames.Add(id, game);
            return id;


            /*
            int newId = 1;

            if (activeGames.Count > 0)
            {
                foreach (var pair in activeGames)
                {
                    if (pair.Key >= newId)
                        newId = pair.Key + 1;
                }
            }
            
            activeGames.Add(newId, new LudoGame(new Diece()));

            return newId;
            */
        }
    }
}
