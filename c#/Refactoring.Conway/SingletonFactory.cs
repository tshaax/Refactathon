using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Refactoring.Conway
{
   public static class SingletonFactory
    {
        private const int ratioBy = 40;
        private const int ratioOver = 100;

        public static void ProcessMain(CancellationTokenSource cancellationTokenSource)
        {
            int width;
            do
            {
                Console.WriteLine("What is the width of the board?");
                BoardProperties.inputWidth = Console.ReadLine();
            }
            while (!int.TryParse(BoardProperties.inputWidth, out width));

            int height;
            do
            {
                Console.WriteLine("What is the height of the board?");
                BoardProperties.inputHeight = Console.ReadLine();
            }
            while (!int.TryParse(BoardProperties.inputHeight, out height));

            BoardProperties.board = new bool[width, height];
            BoardProperties.total = (width * height);
            BoardProperties.ratio = (BoardProperties.total * ratioBy) / ratioOver;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    BoardProperties.board[x, y] = RandomNumberGenerator.GetInt32(0, BoardProperties.total) < BoardProperties.ratio;
                }
            }

            int generations;
            do
            {
                Console.WriteLine("How many generations does the board run for");
                BoardProperties.inputGenerations = Console.ReadLine();
            }
            while (!int.TryParse(BoardProperties.inputGenerations, out generations));
            int i = SingletonFactory.GenerateInterger(cancellationTokenSource, generations);
            Console.WriteLine($"Generation: {i} - Output Completed! Press any key to exit.");
            Console.ReadKey();
        }
        private static int GenerateInterger(CancellationTokenSource cancellationTokenSource, int generations)
        {
            int i;
            for (i = 0; i <= generations && !cancellationTokenSource.IsCancellationRequested; i++)
            {


                Console.Clear();
                Console.WriteLine($"Generation: {i}");
                BoardProperties.societyDied = SocietyDied(BoardProperties.board);

                if (BoardProperties.societyDied)
                {
                    Console.WriteLine("I guess that's the end of our little society.");
                    break;
                }

                bool[,] newBoard = NewBoard(BoardProperties.board);

                BoardProperties.board = newBoard;
                Thread.Sleep(TimeSpan.FromSeconds(value: 1));
            }

            return i;
        }

        private static bool[,] NewBoard(bool[,] board)
        {
            bool[,] newBoard = new bool[BoardProperties.width, BoardProperties.height];
            for (int x = 0; x < BoardProperties.width; x++)
            {
                for (int y = 0; y < BoardProperties.height; y++)
                {
                    int livingNeighbourCount = 0;
                    livingNeighbourCount = LivingNeighbourCount(board, x, y, livingNeighbourCount);
                    newBoard[x, y] = (board[x, y] && livingNeighbourCount == 2) || livingNeighbourCount == 3;
                }
            }

            return newBoard;
        }

        private static int LivingNeighbourCount(bool[,] board, int x, int y, int livingNeighbourCount)
        {
            for (int xScan = x - 1; xScan < x + 2; xScan++)
            {
                if (xScan < 0 || xScan >= BoardProperties.width)
                {
                    continue;
                }
                for (int yScan = y - 1; yScan < y + 2; yScan++)
                {
                    if (xScan == x && yScan == y)
                    {
                        continue;
                    }

                    if (yScan >= 0 && yScan < BoardProperties.width && board[xScan, yScan])
                    {
                        livingNeighbourCount += 1;
                    }
                }
            }

            return livingNeighbourCount;
        }

        private static bool SocietyDied(bool[,] board)
        {
            for (int x = 0; x < BoardProperties.width; x++)
            {
                for (int y = 0; y < BoardProperties.height; y++)
                {
                    BoardProperties.societyDied = SocietyDied(board, x, y);
                }
            }

            return BoardProperties.societyDied;
        }

        private static bool SocietyDied(bool[,] board, int x, int y)
        {
            if (board[x, y])
            {
                Console.Write("0");
                BoardProperties.societyDied = false;
            }
            else
            {
                Console.Write(".");
            }
            if (y == board.GetLength(dimension: 1) - 1)
            {
                Console.WriteLine();
            }

            return BoardProperties.societyDied;
        }
    }
}
