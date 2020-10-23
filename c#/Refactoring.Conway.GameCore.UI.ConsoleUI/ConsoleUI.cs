using System;
using Refactoring.Conway.GameCore.Interfaces;

namespace Refactoring.Conway.GameCore.UI.ConsoleUI
{
    public class ConsoleUI : IGameEngine
    {
        public void DrawGame(GameOfLife world)
        {
            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    Console.Write(world.CurrentTiles[x, y] ? world.Alive : world.Dead);

                    if (y == world.CurrentTiles.GetLength(dimension: 1) - 1)
                    {
                        Console.WriteLine();
                    }
                }
            }
        }

        public void EndGame()
        {
            
        }

        public GameOfLife Next(GameOfLife world)
        {
           return world.Next();
        }
    }
}
