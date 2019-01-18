using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGameEngine;

namespace LudoWebAPI.Models
{
    public class Game
    {
        static List<ILudoGame> activeGames = new List<ILudoGame> { };

        public static void CreateNewGame()
        {
            activeGames.Add(new LudoGame());
        }
    

        
    }
}
