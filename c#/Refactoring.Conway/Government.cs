namespace Refactoring.Conway
{
    public class Government
    {
        private readonly int _width;
        private readonly int _height;
        private bool[,] _board;
        private bool[,] _newBoard;

        public Government(int width, int height, bool[,] board, bool[,] newBoard)
        {
            _width = width;
            _height = height;
            _board = board;
            _newBoard = newBoard;
        }

        public void RunGovernment(bool[,] board, bool[,] newBoard)
        {
            for (int row = 0; row < _width; row++)
            {
                for (int column = 0; column < _height; column++)
                {
                    // Find your living neighbours
                    int livingNeighbourCount = 0;
                    for (int rowScan = row - 1; rowScan < row + 2; rowScan++)
                    {
                        if (AreYouACitizen(_width, rowScan)) // Are You Citizen 
                        {
                            continue;
                        }
                        for (int columnScan = column - 1; columnScan < column + 2; columnScan++)
                        {
                            if (AreYouPayingTaxes(row, column, rowScan, columnScan)) // Are you paying taxes
                            {
                                continue;
                            }

                            if (Urbanize(_height, board, rowScan, columnScan)) // Urbanize
                            {
                                livingNeighbourCount += 1;
                            }
                        }
                    }
                    newBoard[row, column] = SomebodyHasToDieSomebody(board, row, column, livingNeighbourCount);
                }
            }
        }

        private static bool Urbanize(int height, bool[,] board, int xScan, int columnScan)
        {
            return columnScan >= 0 && columnScan < height && board[xScan, columnScan];
        }

        public static bool AreYouACitizen(int width, int rowScan)
        {
            return rowScan < 0 || rowScan >= width;
        }

        public static bool AreYouPayingTaxes(int x, int y, int rowScan, int columnScan)
        {
            return rowScan == x && columnScan == y;
        }

        private static bool SomebodyHasToDieSomebody(bool[,] board, int x, int y, int livingNeighbourCount)
        {
            return (board[x, y] && livingNeighbourCount == 2) || livingNeighbourCount == 3;
        }
    }
}
