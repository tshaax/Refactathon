using System;
using System.Security.Cryptography;
using System.Threading;

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
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }
            Console.CancelKeyPress += OnCancelKeyPress;

            try
            {
                int width;
                string inputWidth;
                do
                {
                    Console.WriteLine("What is the width of the board?");
                    inputWidth = Console.ReadLine();
                }
                while (!int.TryParse(inputWidth, out width));

                int height;
                string inputHeight;
                do
                {
                    Console.WriteLine("What is the width of the board?");
                    inputHeight = Console.ReadLine();
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
                    Console.WriteLine("What is the width of the board?");
                    inputGenerations = Console.ReadLine();
                }
                while (!int.TryParse(inputGenerations, out generations));

                for (int i = 0; i <= generations && !cancellationTokenSource.IsCancellationRequested; i++)
                {
                    Console.WriteLine($"Generation: {0}");
                    Console.Clear();
                    bool societyDied = true;
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (board[x, y])
                            {
                                Console.Write("0");
                                societyDied = false;
                            }
                            else
                            {
                                Console.Write(".");
                            }
                            if (y == board.GetLength(dimension: 1) - 1)
                            {
                                Console.WriteLine();
                            }
                        }
                    }

                    if(societyDied)
                    {
                        Console.WriteLine("I guess that's the end of our little society.");
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
                Console.WriteLine($"Generation: {generations} - Output Completed! Press any key to exit.");
                Console.ReadKey();
            }
            catch (OperationCanceledException)
            {
                //DO NOTHING
            }
            Console.CancelKeyPress -= OnCancelKeyPress;
        }
    }
}
