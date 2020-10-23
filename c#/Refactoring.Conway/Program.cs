using System;
using System.Security.Cryptography;
using System.Threading;

namespace Refactoring.Conway
{
    class Program
    {
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
                bool[,] board = CreateCountry(out int width, out int height);

                int generations = GetGenerationsInput();

                int i = SimulateGovernment(cancellationTokenSource, ref board, width, height, generations);

                Console.WriteLine($"Generation: {i} - Output Completed! Press any key to exit.");
                Console.ReadKey();
            }
            catch (OperationCanceledException)
            {
                //DO NOTHING
            }
            Console.CancelKeyPress -= OnCancelKeyPress;
        }

        private static bool[,] CreateCountry(out int height, out int width)
        {
            string inputHeight;
            do
            {
                Console.WriteLine("What is the height of the board?");
                inputHeight = Console.ReadLine();
            }
            while (!int.TryParse(inputHeight, out height));

            string inputWidth;
            do
            {
                Console.WriteLine("What is the width of the board?");
                inputWidth = Console.ReadLine();
            }
            while (!int.TryParse(inputWidth, out width));

            // Generate board
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

            return board;
        }

        private static int GetGenerationsInput()
        {
            int generations;
            string inputGenerations;
            do
            {
                Console.WriteLine("How many generations does the board run for");
                inputGenerations = Console.ReadLine();
            }
            while (!int.TryParse(inputGenerations, out generations));
            return generations;
        }

        private static int SimulateGovernment(CancellationTokenSource cancellationTokenSource, ref bool[,] board, int width, int height, int generations)
        {
            int i;
            
            People people = new People(width, height, board, true);
            for (i = 0; i <= generations && !cancellationTokenSource.IsCancellationRequested; i++)
            {
                bool societyDied = true;

                Console.Clear();
                Console.WriteLine($"Generation: {i}");

                societyDied = people.BeFruitfulAndMultiply(board, societyDied);
                
                if (societyDied)
                {
                    Console.WriteLine("I guess that's the end of our little society.");
                    break;
                }

                bool[,] newBoard = new bool[width, height];

                var government = new Government(width, height, board, newBoard);
                government.RunGovernment(board, newBoard);

                board = newBoard;
                Thread.Sleep(TimeSpan.FromSeconds(value: 1));
            }

            return i;
        }
    }
}
