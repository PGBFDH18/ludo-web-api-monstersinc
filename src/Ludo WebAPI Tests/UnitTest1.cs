using LudoGameEngine;
using LudoWebAPI.Controllers;
using LudoWebAPI.Models;
using Xunit;

namespace Ludo_WebAPI_Tests
{
    public class UnitTest1
    {
        IGameContainer _gameContainer;
        LudoController _LudoController;

        public UnitTest1()
        {
           _gameContainer = new FakeGameContainer(Diece d, ludoGame game);
           _LudoController = new LudoController(_gameContainer);
        }


        [Fact]
        public void Test1()
        {

        }
    }
}
