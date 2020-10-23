namespace Refactoring.Conway.GameCore.Interfaces
{
    public interface IGameEngine
    {
        void DrawGame(GameOfLife board);
        GameOfLife Next(GameOfLife board);
        void EndGame();
    }
}
