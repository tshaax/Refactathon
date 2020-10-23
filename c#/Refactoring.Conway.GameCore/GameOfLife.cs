using System.Security.Cryptography;
using Refactoring.Conway.GameCore.Interfaces;

namespace Refactoring.Conway.GameCore
{
    public struct GameOfLife
    {
        #region Public
        public int CurrentGeneration { get; set; }
        public bool[,] PreviousTiles { get; set; }
        public bool[,] CurrentTiles { get; set; }
        public char Alive { get; set; }
        public char Dead { get; set; }
        public bool SocietyDead { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public IGameEngine GameEngine { get; set; }
        #endregion

        #region Private
        private int Total { get; }
        #endregion

        public GameOfLife(int width, int height, IGameEngine gameEngine, char alive = '0', char dead = '.')
        {
            Width = width;
            Height = height;
            CurrentGeneration = 0;
            PreviousTiles = new bool[width, height];
            CurrentTiles = new bool[width, height];
            Alive = alive;
            Dead = dead;
            SocietyDead = false;
            Total = Width * Height;
            GameEngine = gameEngine;
            ItStarts();
        }

        public GameOfLife(GameOfLife pregenWorld, IGameEngine gameEngine)
        {
            Width = pregenWorld.Width;
            Height = pregenWorld.Height;
            CurrentGeneration = pregenWorld.CurrentGeneration;
            PreviousTiles = pregenWorld.PreviousTiles;
            CurrentTiles = pregenWorld.CurrentTiles;
            Alive = pregenWorld.Alive;
            Dead = pregenWorld.Dead;
            SocietyDead = pregenWorld.SocietyDead;
            Total = pregenWorld.Width * pregenWorld.Height;
            GameEngine = gameEngine;
        }


        public GameOfLife Next()
        {
            GameOfLife newGeneration = new GameOfLife(Width, Height, GameEngine);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int livingNeighbourCount = 0;
                    for (int xScan = x - 1; xScan < x + 2; xScan++)
                    {
                        if (xScan < 0 || xScan >= Width)
                        {
                            continue;
                        }

                        for (int yScan = y - 1; yScan < y + 2; yScan++)
                        {
                            if (xScan == x && yScan == y)
                            {
                                continue;
                            }

                            if (yScan >= 0 && yScan < Height && CurrentTiles[xScan, yScan])
                            {
                                livingNeighbourCount += 1;
                            }
                        }
                    }

                    newGeneration.PreviousTiles = CurrentTiles;
                    newGeneration.CurrentTiles[x, y] = (CurrentTiles[x, y] && livingNeighbourCount == 2) ||
                                                       livingNeighbourCount == 3;
                }
            }

            return newGeneration;
        }

        public void ItStarts()
        {
            int ratio = (Total * 40) / 100;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    CurrentTiles[x, y] = RandomNumberGenerator.GetInt32(0, Total) < ratio;
                }
            }
        }

        public bool SocietyDied()
        {
            SocietyDead = true;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (CurrentTiles[x, y])
                    {
                        SocietyDead = false;
                    }
                }
            }

            return false;
        }
    }
}
