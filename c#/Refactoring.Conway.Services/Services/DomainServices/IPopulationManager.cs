using Refactoring.Conway.Domain.Models;

namespace Refactoring.Conway.Services.Services.DomainServices
{
    public interface IPopulationManager
    {
        Cell[,] GeneratePopulation(int width, int height);

        void DisplayPopulation(Board board, ref bool isPopulationExtinct, int generationCount);
    }
}
