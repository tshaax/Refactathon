using System;
using System.IO;
using Newtonsoft.Json;
using Refactoring.Conway.GameCore;
using Refactoring.Conway.GameCore.UI.ConsoleUI;

namespace Refactoring.Conway.Testing
{
    public static class Tester
    {
        public static void CreateTests(int tests, string folder)
        {
            Random rnd = new Random(12345);
            for (int i = 0; i < tests; i++)
            {
                int width = rnd.Next(2, 20);
                int height = rnd.Next(2, 20);
                int seed = rnd.Next(0, int.MaxValue);
                int generations = rnd.Next(1, 20);
                //RunGameLoop(width, height, seed, generations, folder);
            }
        }

        public static void RunTests(string folder)
        {
            var files = Directory.GetFiles(folder, "*.test");
            int count = 0;
            foreach (string file in files)
            {
                Console.Write($"\r{count}/{files.Length}");                
                TestCase test = JsonConvert.DeserializeObject<TestCase>(File.ReadAllText(file));
                GameOfLife gameOfLife = new GameOfLife()
                {
                    CurrentTiles = test.StartingBoard,
                    GameEngine = new ConsoleUI(),
                    Width = test.Width,
                    Height = test.Height,
                };                
                for (int i = 0; i < test.Generations.Count; i++)
                {
                    gameOfLife = gameOfLife.Next();
                }
                for(int x = 0; x < test.Width; x++)
                {
                    for(int y = 0; y < test.Height; y++)
                    {
                        if(test.Generations.Count > 0 && gameOfLife.CurrentTiles[x,y] != test.Generations[test.Generations.Count - 1][x,y])
                        {
                            throw new Exception("Oh shit");
                        }
                    }
                }
                count++;
            }
        }
    }
}
