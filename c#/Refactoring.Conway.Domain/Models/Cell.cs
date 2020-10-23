namespace Refactoring.Conway.Domain.Models
{
    public class Cell
    {
        public bool IsAlive { get; set; }

        public Cell(bool isAlive)
        {
            IsAlive = isAlive;
        }

        public char ToChar()
        {
            return IsAlive ? '0' : '.';
        }

        public override string ToString()
        {
            return ToChar().ToString();
        }
    }
}