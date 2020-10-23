using System;
using System.Security.Cryptography;
using System.Threading;
using static System.Console;

namespace Refactoring.Conway
{
    class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        static void Main(string[] args)
        {
            CancelKeyPress += OnCancelKeyPress;

            try
            {
                var width = ReadInteger("What is the width of the board?");
                var height = ReadInteger("What is the height of the board?");

                var board = new bool[width, height];
                var total = width * height;
                var ratio = total * 4 / 10;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        board[x, y] = RandomNumberGenerator.GetInt32(0, total) < ratio;
                    }
                }


                var generations = ReadInteger("How many generations does the board run for");
                int i;
                for (i = 0; i <= generations && !CancellationTokenSource.IsCancellationRequested; i++)
                {
                    var societyDied = true;

                    Clear();
                    WriteLine($"Generation: {i}");
                    for (var x = 0; x < width; x++)
                    {
                        for (var y = 0; y < height; y++)
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

                    if (societyDied)
                    {
                        WriteLine("I guess that's the end of our little society.");
                        break;
                    }

                    var newBoard = new bool[width, height];
                    for (var x = 0; x < width; x++)
                    {
                        for (var y = 0; y < height; y++)
                        {
                            var livingNeighbourCount = 0;
                            for (var xScan = x - 1; xScan < x + 2; xScan++)
                            {
                                if (xScan < 0 || xScan >= width)
                                {
                                    continue;
                                }
                                for (var yScan = y - 1; yScan < y + 2; yScan++)
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
                            newBoard[x, y] = board[x, y] && livingNeighbourCount == 2 || livingNeighbourCount == 3;
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

        private static int ReadInteger(string output)
        {
            int dimension;
            string inputDimension;
            do
            {
                WriteLine(output);
                inputDimension = ReadLine();
            }
            while (!int.TryParse(inputDimension, out dimension));

            return dimension;
        }

        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            CancellationTokenSource.Cancel();
        }
    }
}
