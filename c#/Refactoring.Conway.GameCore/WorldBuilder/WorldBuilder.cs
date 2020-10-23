using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Refactoring.Conway.GameCore.Interfaces;

namespace Refactoring.Conway.GameCore.WorldBuilder
{
    public static class WorldBuilder
    {
        static readonly Random Random = new Random();

        public static Dictionary<string, GameOfLife> PregenWorlds = new Dictionary<string, GameOfLife>();

        public static GameOfLife GenerateWorld(IGameEngine renderEngine)
        {
            GameOfLife generatedWorld = new GameOfLife(Random.Next(0, 100), Random.Next(0, 100), renderEngine, alive: '0', dead: '.');
            return new GameOfLife(generatedWorld, generatedWorld.GameEngine);
        }

        public static void SaveWorld(string gameName, GameOfLife gameObject)
        {
            var serializedWorld = JsonConvert.SerializeObject(gameObject);
            string worldPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (!Directory.Exists(Path.Combine(worldPath, "GameOfLife")))
            {
                Directory.CreateDirectory(Path.Combine(worldPath, "GameOfLife"));
            }

            using (StreamWriter outputFile =
                new StreamWriter(Path.Combine(worldPath, "GameOfLife", $"{gameName}.pregen")))
            {
                outputFile.Write(serializedWorld);
            }
        }

        public static void LoadWorlds()
        {
            string worldPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = Directory.GetFiles(Path.Combine(worldPath, "GameOfLife"), "*.pregen");
            foreach (string file in files)
            {
                PregenWorlds.Add(file, JsonConvert.DeserializeObject<GameOfLife>(File.ReadAllText(file)));
            }
        }

    }
}
