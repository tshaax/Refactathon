using System;

namespace Refactoring.Conway
{
    public class People
    {
        int _width; int _height; bool[,] _board; bool _societyDied;

        public People(int width, int height, bool[,] board, bool societyDied)
        {
            _societyDied = societyDied;
            _width = width;
            _height = height;
            _board = board;
        }
        
        // Thats What She Said
        public bool BeFruitfulAndMultiply(bool[,] board, bool societyDied)
        {
            _board = board;
            _societyDied = societyDied;

            for (int row = 0; row < _width; row++)
            {
                _societyDied = BeFruitful(_height, _board, _societyDied, row);
            }

            return _societyDied;
        }

        public bool BeFruitful(int height, bool[,] board, bool societyDied, int row)
        {
            for (int column = 0; column < height; column++)
            {
                if (board[row, column]) // Can be Fruitful? 
                {
                    Console.Write("x");
                    societyDied = false; // Live
                }
                else
                {
                    Console.Write("."); // Die
                }
                if (column == board.GetLength(dimension: 1) - 1)
                {
                    Console.WriteLine();
                }
            }

            return societyDied;
        }
    }
}
