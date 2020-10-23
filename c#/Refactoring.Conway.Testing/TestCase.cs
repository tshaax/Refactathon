using System.Collections.Generic;

namespace Refactoring.Conway.Testing
{
    public class TestCase
    {
        public int RandomSeed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int GenerationAmount { get; set; }
        public bool[,] StartingBoard { get; set; }
        public List<bool[,]> Generations { get; set; } = new List<bool[,]>();
    }
}
