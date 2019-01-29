using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
namespace LudoConsoleGame

{
    public class LudoGameAPIProcessor
    {
        private RestClient client = new RestClient("https://localhost:44365/");


        public string CreateNewGame()
        {
            var route = "api/ludo";

            var response = client.Execute(new RestRequest(route, Method.POST));

            return response.Content;
        }

        public string AddNewPalyer(int gameId, string name, string color)
        {
            var route = "api/ludo/{gameId}/player";

            var request = new RestRequest(route, Method.POST);

            request.AddUrlSegment("gameId", gameId);
            request.AddQueryParameter("name", name);
            request.AddQueryParameter("color", color);

            var response = client.Execute(request);

            return response.Content;

        }

        public string StartGame(int gameId)
        {
            var route = "api/ludo/{gameId}";

            var request = new RestRequest(route, Method.PUT);

            request.AddUrlSegment("gameId", gameId);

            var response = client.Execute(request);

            return response.Content;

        }

        public string RollDiece(int gameId)
        {
            var route = "api/ludo/{gameId}/roll";

            var request = new RestRequest(route, Method.GET);

            request.AddUrlSegment("gameId", gameId);

            var response = client.Execute(request);

            return response.Content;

        }

        public Player GetCurrentPlayer(int gameId)
        {
            var route = "api/ludo/{gameId}/player/current";

            var request = new RestRequest(route, Method.GET);

            request.AddUrlSegment("gameId", gameId);

            var response = client.Execute(request);


            try
            {
                return JsonConvert.DeserializeObject<Player>(response.Content);
            }
            catch
            {
                return null;
            }

        }

        public string MovePiece(int gameId, int pieceId, int roll)
        {
            var route = "api/ludo/{gameId}/movepiece";

            var request = new RestRequest(route, Method.PUT);

            request.AddUrlSegment("gameId", gameId);
            request.AddQueryParameter("pieceId", pieceId.ToString());
            request.AddQueryParameter("roll", roll.ToString());

            var response = client.Execute(request);

            return response.Content;

        }

        public string EndTurn(int gameId, int currentPlayeId)
        {
            var route = "api/ludo/{gameId}/player/{playerId}/endturn";

            var request = new RestRequest(route, Method.PUT);

            request.AddUrlSegment("gameId", gameId);
            request.AddUrlSegment("playerId", currentPlayeId);

            var response = client.Execute(request);

            return response.Content;
        }

        public Player GetWinner(int gameId)
        {
            var route = "api/ludo/{gameId}/winner";

            var request = new RestRequest(route, Method.PUT);

            request.AddUrlSegment("gameId", gameId);

            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<Player>(response.Content);
        }

        public Dictionary<int, LudoGame> ActiveGames()
        {
            var route = "api/ludo";

            var request = new RestRequest(route, Method.GET);

            var response = client.Execute(request);

            try
            {
                return JsonConvert.DeserializeObject<Dictionary<int, LudoGame>>(response.Content);
            }
            catch
            {
                return null;

            }

        }

        public int NumberOfPlayersAdded(int gameId)
        {
            var route = "api/ludo/{gameId}/player";

            var request = new RestRequest(route, Method.GET);
            request.AddUrlSegment("gameId", gameId);

            var response = client.Execute(request);

            try
            {
                return JsonConvert.DeserializeObject<Player[]>(response.Content).Length;
            }
            catch
            {
                return 0;
            }
        }

        public LudoGame GameById(int gameId)
        {
            var route = "api/ludo/{gameId}";
            var request = new RestRequest(route, Method.GET);
            request.AddUrlSegment("gameId", gameId);
            var response = client.Execute(request);

            try
            {
                return JsonConvert.DeserializeObject<LudoGame>(response.Content);
            }
            catch
            {
                return null;
            }

        }

        public string DeleteGame(int gameId)
        {
            var route = "{gameId}/delete";
            var request = new RestRequest(route, Method.DELETE);
            request.AddUrlSegment("gameId", gameId);

            var response = client.Execute(request);
            return response.Content;
        }

    }
}