using Refactoring.Conway.Domain.Exceptions;

namespace Refactoring.Conway.Domain.Models
{
    public class Board
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Cell[,] Cells { get; set; }


        public Board(int width, int height, Cell[,] cells)
        {
            Width = width;
            Height = height;
            Cells = cells ?? throw new InvalidCellException("NULL reference passed with cells");
        }
        
        public int GetNumberOfCellNeighbours(int widthIndex, int heightIndex)
        {
            int livingNeighbourCount = 0;
            for (int xScan = widthIndex - 1; xScan < widthIndex + 2; xScan++)
            {
                if (xScan < 0 || xScan >= Width)
                {
                    continue;
                }

                for (int yScan = heightIndex - 1; yScan < heightIndex + 2; yScan++)
                {
                    if (xScan == widthIndex && yScan == heightIndex)
                    {
                        continue;
                    }

                    if (yScan >= 0 && yScan < Height && Cells[xScan, yScan].IsAlive)
                    {
                        livingNeighbourCount += 1;
                    }
                }
            }

            return livingNeighbourCount;
        }
    }
}