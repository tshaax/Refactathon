using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Refactoring.Conway
{
    public class Conway
    {
        public Conway(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Board = new bool[this.Width, this.Height];
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public bool[,] Board { get; set; }

        public bool[,] GetNewBoard()
        {
            bool[,] newBoard = new bool[this.Width, this.Height];

            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    var livingNeighbourCount = 0;

                    for (var xScan = x - 1; xScan < x + 2; xScan++)
                    {
                        if (xScan < 0 || xScan >= this.Width)
                        {
                            continue;
                        }

                        for (var yScan = y - 1; yScan < y + 2; yScan++)
                        {
                            if (xScan == x && yScan == y)
                            {
                                continue;
                            }
                            if (yScan >= 0 && yScan < this.Height && Board[xScan, yScan])
                            {
                                livingNeighbourCount += 1;
                            }
                        }
                    }
                    newBoard[x, y] = (Board[x, y] && livingNeighbourCount == 2) || livingNeighbourCount == 3;
                }
            }
            return newBoard;
        }

        public bool[,] GetBoard()
        {
            var total = (this.Width * this.Height);
            var ratio = (total * 40) / 100;

            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    Board[x, y] = RandomNumberGenerator.GetInt32(0, total) < ratio;
                }
            }
            return Board;
        }

        public bool SocietyDied()
        {
            var societyDied = true;
            var boardLength = this.Board.GetLength(1) - 1;

            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    if (this.Board[x, y])
                    {
                        Console.Write("0");
                        societyDied = false;
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
            return societyDied;
        }
    }
}
