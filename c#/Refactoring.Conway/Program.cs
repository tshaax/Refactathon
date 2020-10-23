using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Refactoring.Conway.Common;
using Refactoring.Conway.GameCore;
using Refactoring.Conway.GameCore.Interfaces;
using Refactoring.Conway.GameCore.UI.ConsoleUI;
using Refactoring.Conway.GameCore.UI.UnityUI;
using Refactoring.Conway.GameCore.WorldBuilder;
using Serilog;

namespace Refactoring.Conway
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        static void Main(string[] args)
        {
            BuildConfiguration(args);
            BuildLogger();
           
            Console.CancelKeyPress += OnCancelKeyPress;
            try
            {
                var gameOfLife = SetupGameBoard(out var generations);
                int currentGeneration;
                for (currentGeneration = 0; currentGeneration <= generations && !CancellationTokenSource.IsCancellationRequested; currentGeneration++)
                {
                    Console.Clear();
                    Console.WriteLine($@"{Localization.GenerationLabel}: {currentGeneration}");
                    if (gameOfLife.SocietyDied())
                    {
                        Console.Clear();
                        Console.WriteLine(Localization.EndGameMessage);                        
                        break;
                    }
                    gameOfLife.GameEngine.DrawGame(gameOfLife);
                    gameOfLife = gameOfLife.GameEngine.Next(gameOfLife);
                    Thread.Sleep(TimeSpan.FromSeconds(value: Utilities.ConfigValueOrDefault(Configuration, ApplicationSettingNames.DefaultGenerationLifeTime, 1)));
                }
                Console.WriteLine($@"{Localization.GenerationLabel}: {currentGeneration} - {Localization.AppExitMessage}");
                gameOfLife.GameEngine.EndGame();
                Console.ReadKey();
            }
            catch (OperationCanceledException)
            {
                Log.Information(Localization.UserCancelled);
            }
            catch (Exception ex)
            {
                Log.Error(Localization.UnhandledException, ex);
            }

            Console.CancelKeyPress -= OnCancelKeyPress;
        }


        #region Private Methods

        private static GameOfLife SetupGameBoard(out int generations)
        {
            int width = Utilities.GetUserInputInt(Localization.BoardWidthPrompt,
                Utilities.ConfigValueOrDefault(Configuration, ApplicationSettingNames.DefaultBoardWidth, 100));
            int height = Utilities.GetUserInputInt(Localization.BoardHeightPrompt,
                Utilities.ConfigValueOrDefault(Configuration, ApplicationSettingNames.DefaultBoardHeight, 100));
            generations = Utilities.GetUserInputInt(Localization.GameGenerationCountPrompt,
                Utilities.ConfigValueOrDefault(Configuration, ApplicationSettingNames.DefaultGenerations, 10));

            IGameEngine engine = getGameEngineFromConfig(Utilities.ConfigValueOrDefault(Configuration, ApplicationSettingNames.RenderEngine, "Console"));

            return new GameOfLife(width, height, engine);
        }

        private static IGameEngine getGameEngineFromConfig(string configValueOrDefault)
        {
            if (configValueOrDefault.Equals("Unity", StringComparison.InvariantCultureIgnoreCase))
            {
                return new UnityUI();
            }
            return new ConsoleUI();
        }


        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs userArgs)
        {
            userArgs.Cancel = true;
            CancellationTokenSource.Cancel();
        }

        private static void BuildConfiguration(string[] args)
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .AddCommandLine(args)
               .Build();
        }

        private static void BuildLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("life.log")
                .CreateLogger();
        }
        #endregion
    }
}
