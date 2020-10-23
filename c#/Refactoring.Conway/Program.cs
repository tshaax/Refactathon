using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

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
            Console.CancelKeyPress += OnCancelKeyPress;

            try
            {
                var (width, height, generations) = GetBoardDimensions();
                bool[,] board = new bool[width, height];
                bool[,] board1 = new bool[width, height];
                int total = (width * height);
                int ratio = (total * 40) / 100;

                Parallel.For(0, width, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                    x => { Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = 1 }, y => { board[x, y] = RandomNumberGenerator.GetInt32(0, total) < ratio; }); });



                int i;
                for (i = 0; i <= generations && !cancellationTokenSource.IsCancellationRequested; i++)
                {
                    bool societyDied = true;

                    Console.Clear();
                    Console.WriteLine($"Generation: {i}");

                    Parallel.For(0, width, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                        x =>
                        {
                            Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                                              y =>
                                              {
                                                  societyDied = board[x, y] == true ? false : true;
                                                  var printText = board[x, y] == true ? "0" : ".";
                                                  Console.Write(printText);
                                                  if (y == board.GetLength(dimension: 1) - 1)
                                                  {
                                                      Console.WriteLine();
                                                  }
                                              }
                                         );
                        });


                    if (societyDied)
                    {
                        Console.WriteLine("I guess that's the end of our little society.");
                        break;
                    }

                    bool[,] newBoard = new bool[width, height];
                    
                    Parallel.For(0, width, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                       x =>
                       {
                           Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                                             y =>
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
                                        );
                       });

                    board = newBoard;
                    Thread.Sleep(TimeSpan.FromSeconds(value: 1));
                }
                Console.WriteLine($"Generation: {i} - Output Completed! Press any key to exit.");
                Console.ReadKey();
            }
            catch (OperationCanceledException)
            {
                //DO NOTHING
            }
            Console.CancelKeyPress -= OnCancelKeyPress;
        }

        public static (int, int, int) GetBoardDimensions()
        {
            List<string> dimensions;
            int width;
            int height;
            int generations;
            do
            {
                Console.WriteLine(@"Please enter the width and height of the board; And the number of generations the board run for.{0}NB: Values must be separated by comma("","")", Environment.NewLine);
                dimensions = Console.ReadLine().Split(',').ToList();
            }
            while (!int.TryParse(dimensions.Count == 3 ? dimensions?[0] : "invalid", out width) || !int.TryParse(dimensions.Count == 3 ? dimensions?[1] : "invalid", out height)
                    || !int.TryParse(dimensions.Count == 3 ? dimensions?[1] : "invalid", out generations));
            return (width, height, generations);
        }
    }
}
