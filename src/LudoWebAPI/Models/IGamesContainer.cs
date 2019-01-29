using LudoGameEngine;
using System.Collections.Generic;

namespace LudoWebAPI.Models
{
    /// <summary>
    /// Inteface for GameContainer
    /// </summary>
    public interface IGameContainer
    {


        Dictionary<int, ILudoGame> Gamesloader();
        int AddNewGame();
    }


}
