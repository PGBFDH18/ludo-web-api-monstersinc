using LudoGameEngine;
using LudoWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ludo_WebAPI_Tests
{
    class FakeGameContainer : IGameContainer
    {
        private readonly Dictionary<int, ILudoGame> _activeGames;
        private readonly Diece _diece;
        private readonly LudoGame _ludoGame;


        public FakeGameContainer(Diece diece, LudoGame ludoGame)
        {
            _activeGames = new Dictionary<int, ILudoGame>();
            _diece = diece;
            _ludoGame = ludoGame;
        }



        public Dictionary<int, ILudoGame> Gamesloader()
        {

            return _activeGames;
        }


        public void AddNewGame()
        {
            int newId = 1;


            if (_activeGames.Count > 0)
            {
                foreach (var pair in _activeGames)
                {
                    if (pair.Key >= newId)
                        newId = pair.Key + 1;
                }
            }

            _activeGames.Add(newId, _ludoGame);
        }
    }
}
