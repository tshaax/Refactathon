using Newtonsoft.Json;
using Refactoring.Conway.GameCore.Interfaces;
using RestSharp;
using System;
using System.Diagnostics;
using System.Threading;

namespace Refactoring.Conway.GameCore.UI.UnityUI
{
    public class UnityUI : IGameEngine
    {
        RestClient client = new RestClient("http://localhost:8181/ReceiveData/");
        Process unity;
        public UnityUI()
        {
            unity = Process.Start("UnityUI/Conway.exe");
            Thread.Sleep(5000);
        }

        public void DrawGame(GameOfLife board)
        {
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(board.CurrentTiles), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        public void EndGame()
        {
            unity.Kill();
        }

        public GameOfLife Next(GameOfLife board)
        {
            return board.Next();
        }
    }
}
