using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGameEngine;

namespace LudoWebAPI.Models
{
    public interface IGameContainer
    {
        Dictionary<int, ILudoGame> Gamesloader();
    }
   

    public class GameContainer: IGameContainer
    {
        private Dictionary<int, ILudoGame> _activeGames;

        // constructor that instansiate the dictionary 
        public GameContainer()
        {
            _activeGames = new Dictionary<int, ILudoGame>();
        }

        // rutuns the dictionary
        public Dictionary<int, ILudoGame> Gamesloader()
        {
            
            return _activeGames;
        }        
    }
}
