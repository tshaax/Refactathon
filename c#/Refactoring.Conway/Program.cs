using System;
using System.Threading;
using Refactoring.Conway.Domain.Models;
using Refactoring.Conway.Services.Services.ApplicationServices;
using Refactoring.Conway.Services.Services.DomainServices;

namespace Refactoring.Conway
{
    class Program
    {
        static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        static void Main(string[] args)
        {
            Console.CancelKeyPress += OnCancelKeyPress;

            try
            {
                // Initialize all the services we the application depends on
                ConsoleInteractionService interactiveService = new ConsoleInteractionService();
                PopulationManager populationManager = new PopulationManager(interactiveService);

                // Set board/population parameters
                int width = interactiveService.PromptIntegerQuestion("What is the width of the board?");
                int height = interactiveService.PromptIntegerQuestion("What is the height of the board?");
                int generations = interactiveService.PromptIntegerQuestion("How many generations does the board run for?");
                
                // Initialize board and set initial population
                Board board = new Board(width, height, populationManager.GeneratePopulation(width, height));

                // Update population
                populationManager.UpdatePopulation(board, generations, CancellationTokenSource);
                interactiveService.ReadKey();
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
            CancellationTokenSource.Cancel();
        }
    }
}
