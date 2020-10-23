using System;
using System.Threading;

namespace Refactoring.Conway
{
    class Program
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        static void Main(string[] args)
        {
            Console.CancelKeyPress += OnCancelKeyPress;

            try
            {
                var width = InputWidth();
                var height =InputHeight();
                var generations = InputGenerations();

                Conway conway = new Conway(width, height);

                conway.Board = conway.GetBoard();
                
                int i;
                for (i = 0; i <= generations && !cancellationTokenSource.IsCancellationRequested; i++)
                {
                    Console.Clear();

                    Console.WriteLine($"Generation: {i}");

                    bool societyDied = conway.SocietyDied();

                    if (societyDied)
                    {
                        Console.WriteLine("I guess that's the end of our little society.");

                        break;
                    }

                    conway.Board = conway.GetNewBoard();

                    Thread.Sleep(TimeSpan.FromSeconds(1));
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

        

        private static int InputHeight()
        {
            string inputHeight;
            int height;
            do
            {
                Console.WriteLine("What is the height of the board?");
                inputHeight = Console.ReadLine();

            } while (!int.TryParse(inputHeight, out height));

            return height;
        }

        private static int InputWidth()
        {
            string inputWidth;
            int width;

            do
            {
                Console.WriteLine("What is the width of the board?");
                inputWidth = Console.ReadLine();

            } while (!int.TryParse(inputWidth, out width));

            return width;
        }

        private static int InputGenerations()
        {
            string inputGenerations;
            int generations;

            do
            {
                Console.WriteLine("How many generations does the board run for");
                inputGenerations = Console.ReadLine();

            } while (!int.TryParse(inputGenerations, out generations));

            return generations;

        }


        #region Events
        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;

            cancellationTokenSource.Cancel();
        }
        #endregion
    }
}
