using GameOfLife;
using System;
using System.Security.Cryptography;
using System.Threading;
using static System.Console;

namespace Refactoring.Conway
{
    class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private const int PercentageBoardToStartAlive = 40;

        static void Main(string[] args)
        {
            CancelKeyPress += OnCancelKeyPress;

            try
            {
                var width = ReadInteger("What is the width of the board?");
                var height = ReadInteger("What is the height of the board?");
                var generations = ReadInteger("How many generations does the board run for?");
                var board = new GameOfLife.GameOfLife(width, height, generations);

                var gensCompleted = 0;
                var stringBoards = board.BoardsToString(board.Run(CancellationTokenSource.Token));
                foreach (var output in stringBoards)
                {
                    Clear();
                    WriteLine($"Generation: {gensCompleted}");
                    WriteLine(output);
                    Thread.Sleep(TimeSpan.FromSeconds(value: 1));
                    gensCompleted++;
                }
                if (gensCompleted <= generations)
                    WriteLine("I guess that's the end of our little society.");
                WriteLine($"Generation: {gensCompleted} - Output Completed! Press any key to exit.");
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
            while (!int.TryParse(inputDimension, out dimension) || dimension <= 0);

            return dimension;
        }

        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            CancellationTokenSource.Cancel();
        }
    }
}
