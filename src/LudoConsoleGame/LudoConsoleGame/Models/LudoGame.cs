using Newtonsoft.Json;
using System.Collections.Generic;


namespace LudoConsoleGame
{
    public class LudoGame
    {
        [JsonProperty(PropertyName = "players")]
        public List<Player> _players = new List<Player>();

        [JsonProperty(PropertyName = "gameState")]
        public string _gameState = "NotStarted";

        [JsonProperty(PropertyName = "currentPlayerId")]
        public int currentPlayerId = 0;
    }
}
