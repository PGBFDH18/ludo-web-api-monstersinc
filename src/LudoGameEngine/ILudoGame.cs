namespace LudoGameEngine
{
    public interface ILudoGame
    {
        bool StartGame();
        Player AddPlayer(string name, PlayerColor color);
        Player[] GetPlayers();
        GameState GetGameState();
        //void StartTurn(Player player);

        int RollDiece();

        int MovePiece(Player player, int pieceId, int numberOfFields);
        void EndTurn(Player player);

        Player GetCurrentPlayer();
        Piece[] GetAllPiecesInGame();

        Player GetWinner();
    }
}