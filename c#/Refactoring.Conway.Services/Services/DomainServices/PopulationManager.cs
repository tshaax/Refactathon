using System;
using System.Security.Cryptography;
using System.Threading;
using Refactoring.Conway.Domain.Models;
using Refactoring.Conway.Services.Extensions;
using Refactoring.Conway.Services.Services.ApplicationServices;

namespace Refactoring.Conway.Services.Services.DomainServices
{
    public class PopulationManager : IPopulationManager
    {
        private readonly IInteractionService _interactionService;

        public PopulationManager(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public Cell[,] GeneratePopulation(int width, int height)
        {
            Cell[,] population = new Cell[width, height];
            int total = (width * height);
            int ratio = (total * 40) / 100;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    population[x, y] = new Cell(RandomNumberGenerator.GetInt32(0, total) < ratio);
                }
            }

            return population;
        }

        public void DisplayPopulation(Board board, ref bool isPopulationExtinct, int generationCount)
        {
            _interactionService.DisplayMessage($"Generation: {generationCount}", true);

            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    _interactionService.PrintCharacter(board.Cells[x, y].ToChar());
                    
                    isPopulationExtinct = isPopulationExtinct ? board.Cells[x, y].IsAlive: isPopulationExtinct;

                    if (y == board.Height -1) 
                        Console.WriteLine();
                }
            }
        }

        public void UpdatePopulation(Board board, int generations, CancellationTokenSource cancellationTokenSource)
        {
            int generationCount;
            for (generationCount = 1; generationCount <= generations && !cancellationTokenSource.IsCancellationRequested; generationCount++)
            {
                bool isPopulationExtinct = true;

                DisplayPopulation(board, ref isPopulationExtinct, generationCount);

                if (isPopulationExtinct)
                {
                    _interactionService.DisplayMessage("I guess that's the end of our little society.");
                    break;
                }

                for (int x = 0; x < board.Width; x++)
                {
                    for (int y = 0; y < board.Height; y++)
                    {
                        int numberOfNeighbours = board.GetNumberOfCellNeighbours(x, y);
                        board.Cells[x, y].UpdateCellMortality(numberOfNeighbours);
                    }
                }

                Thread.Sleep(TimeSpan.FromSeconds(value: 1));
            }

            _interactionService.DisplayMessage($"Generation: {generationCount - 1} - Output Completed! Press any key to exit.");
        }


    }
}