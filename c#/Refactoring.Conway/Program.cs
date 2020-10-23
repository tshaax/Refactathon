using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;

namespace Refactoring.Conway
{
    public class Program
    {
        //TODO: Refactor this

   
        public static void Main(string[] args)
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
                SingletonFactory.ProcessMain(cancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                //DO NOTHING
            }
            Console.CancelKeyPress -= OnCancelKeyPress;
        }

      


    }

    
    
}
