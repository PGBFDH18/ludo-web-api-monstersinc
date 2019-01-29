using Newtonsoft.Json;
namespace LudoConsoleGame

{
    public class Player
    {

        [JsonProperty(PropertyName = "playerId")]
        public int PlayerId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "playercolor")]
        public string PlayerColor { get; set; }

        [JsonProperty(PropertyName = "pieces")]
        public Piece[] Pieces { get; set; }

        [JsonProperty(PropertyName = "offset")]
        public int Offset
        {
            get
            {
                return PlayerId * 13;
            }
        }
    }
}
