using Refactoring.Conway.Domain.Models;

namespace Refactoring.Conway.Services.Extensions
{
    public static class CellExtensions
    {
        public static void UpdateCellMortality(this Cell cell, int numberOfNeighbour)
        {
            cell.IsAlive = (cell.IsAlive && numberOfNeighbour == 2) || numberOfNeighbour == 3;
        }
    }
}
