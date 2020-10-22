using System;
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

                //CODE GOES HERE
                while(!cancellationTokenSource.IsCancellationRequested)
                {
                    Console.In.ReadLine();
                }
            }
            catch (OperationCanceledException)
            {
                //DO NOTHING
            }
            Console.CancelKeyPress -= OnCancelKeyPress;
        }
    }
}
