using LudoGameEngine;
using System.Collections.Generic;
namespace LudoWebAPI.Models
{

    /// <summary>
    /// This class creates a collection of games
    /// </summary>
    public class GameContainer : IGameContainer
    {
        private readonly Dictionary<int, ILudoGame> _activeGames;
        private readonly IDiece _diece;

        /// <summary>
        /// A constructor that inisiate other classes through DI
        /// </summary>
        /// <param name="diece"></param>
        /// <param name="ludoGame"></param>        
        public GameContainer(IDiece diece)
        {
            _diece = diece;
            _activeGames = new Dictionary<int, ILudoGame>();


        }

        /// <summary>
        /// Returns list of active games 
        /// </summary>
        /// <returns>List of active games</returns>
        public Dictionary<int, ILudoGame> Gamesloader()
        {

            return _activeGames;
        }

        /// <summary>
        /// Creates new game 
        /// </summary>
        public int AddNewGame()
        {
            int newId = 1;

            //sets an incrimenting id for each new game
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
