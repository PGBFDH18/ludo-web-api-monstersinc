using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGameEngine;

namespace LudoWebAPI.Models
{
    public class Game
    {
        public static Dictionary<int, LudoGame> activeGames = new Dictionary<int, LudoGame> { };

        public static int CreateNewGame()
        {
            int newId = 1;

            if (activeGames.Count > 0)
            {
                foreach (var pair in activeGames)
                {
                    if (pair.Key >= newId)
                        newId = pair.Key + 1;
                }
            }
            
            activeGames.Add(newId, new LudoGame());

            return newId;
        }
    }
}
