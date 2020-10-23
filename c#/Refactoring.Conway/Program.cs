using System;
using System.Security.Cryptography;
using System.Threading;
using static System.Console;

namespace Refactoring.Conway
{
    class Program
    {
        //TODO: Refactor this
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
            {
                args.Cancel = true;
                cancellationTokenSource.Cancel();
            }
            CancelKeyPress += OnCancelKeyPress;

            try
            {
                int width;
                string inputWidth;
                do
                {
                    WriteLine("What is the width of the board?");
                    inputWidth = ReadLine();
                }
                while (!int.TryParse(inputWidth, out width));

                int height;
                string inputHeight;
                do
                {
                    WriteLine("What is the height of the board?");
                    inputHeight = ReadLine();
                }
                while (!int.TryParse(inputHeight, out height));

                bool[,] board = new bool[width, height];
                int total = (width * height);
                int ratio = (total * 40) / 100;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        board[x, y] = RandomNumberGenerator.GetInt32(0, total) < ratio;
                    }
                }

                int generations;
                string inputGenerations;
                do
                {
                    WriteLine("How many generations does the board run for");
                    inputGenerations = ReadLine();
                }
                while (!int.TryParse(inputGenerations, out generations));
                int i;
                for (i = 0; i <= generations && !cancellationTokenSource.IsCancellationRequested; i++)
                {
                    bool societyDied = true;

                    Clear();
                    WriteLine($"Generation: {i}");
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (board[x, y])
                            {
                                Write("0");
                                societyDied = false;
                            }
                            else
                            {
                                Write(".");
                            }
                            if (y == board.GetLength(dimension: 1) - 1)
                            {
                                WriteLine();
                            }
                        }
                    }

                    if(societyDied)
                    {
                        WriteLine("I guess that's the end of our little society.");
                        break;
                    }

                    bool[,] newBoard = new bool[width, height];
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            int livingNeighbourCount = 0;
                            for (int xScan = x - 1; xScan < x + 2; xScan++)
                            {
                                if (xScan < 0 || xScan >= width)
                                {
                                    continue;
                                }
                                for (int yScan = y - 1; yScan < y + 2; yScan++)
                                {
                                    if (xScan == x && yScan == y)
                                    {
                                        continue;
                                    }

                                    if (yScan >= 0 && yScan < width && board[xScan, yScan])
                                    {
                                        livingNeighbourCount += 1;
                                    }
                                }
                            }
                            newBoard[x, y] = (board[x, y] && livingNeighbourCount == 2) || livingNeighbourCount == 3;
                        }
                    }

                    board = newBoard;
                    Thread.Sleep(TimeSpan.FromSeconds(value: 1));
                }
                WriteLine($"Generation: {i} - Output Completed! Press any key to exit.");
                ReadKey();
            }
            catch (OperationCanceledException)
            {
                //DO NOTHING
            }
            CancelKeyPress -= OnCancelKeyPress;
        }
    }
}
