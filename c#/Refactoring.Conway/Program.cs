using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;

namespace MyVersion
{
    class Program
    {
        //TODO: Refactor this

        private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static void Main(string[] args)
        {
            GameOfLife gameOfLife = new GameOfLife();
            Console.CancelKeyPress += OnCancelKeyPress;

            try
            {

                gameOfLife.SetWidthAndHeight(boardWidthInput: "What is the width of the board?", boardHeightInput: "What is the height of the board?");
                gameOfLife.GenerateBoard();

                int generations = gameOfLife.ReadUserInput("How many generations does the board run for");
                int i;
                for (i = 0; i <= generations && !cancellationTokenSource.IsCancellationRequested; i++)
                {
                    //if current society returns false then our society is dead hence break
                    if (!gameOfLife.CurrentSociety(i)) break;

                    //if our society is alive, add new society based on the rules 
                    gameOfLife.Board = gameOfLife.GetNewBoard();
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

        static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

    }

    public class GameOfLife
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool[,] Board { get; set; }

        public GameOfLife()
        {
            Width = 50;
            Height = 50;
            Board = new bool[Width, Height];
        }

        public int ReadUserInput(string outputText)
        {
            string inputString;
            int userInput;
            do
            {
                Console.WriteLine(outputText);
                inputString = Console.ReadLine();
            }
            while (!int.TryParse(inputString, out userInput));

            return userInput;
        }

        public void GenerateBoard()
        {
            Board = new bool[Width, Height];
            int total = (Width * Height);
            int ratio = (total * 40) / 100;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Board[x, y] = RandomNumberGenerator.GetInt32(0, total) < ratio;
                }
            }
        }

        public bool CurrentSociety(int i)
        {
            bool societyIsAlive = false;

            Console.Clear();
            Console.WriteLine($"Generation: {i}");
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Board[x, y])
                    {
                        Console.Write("0");
                        societyIsAlive = true;
                    }
                    else
                    {
                        Console.Write(".");
                    }
                    if (y == Board.GetLength(dimension: 1) - 1)
                    {
                        Console.WriteLine();
                    }
                }
            }
            if (!societyIsAlive)
            {
                Console.WriteLine("I guess that's the end of our little society.");

            }
            return societyIsAlive;
        }

        public bool[,] GetNewBoard()
        {
            var newBoard = new bool[Width, Height];

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

                            if (yScan >= 0 && yScan < Width && Board[xScan, yScan])
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

        public void SetWidthAndHeight(string boardWidthInput, string boardHeightInput)
        {
            Width = ReadUserInput(boardWidthInput);
            Height = ReadUserInput(boardHeightInput);
        }
    }
}
