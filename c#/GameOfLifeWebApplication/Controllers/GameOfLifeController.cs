using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameOfLife;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameOfLifeWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameOfLifeController : ControllerBase
    {
        private readonly ILogger<GameOfLifeController> _logger;

        public GameOfLifeController(ILogger<GameOfLifeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get(int width, int height, int generations, CancellationToken token)
        {
            var gameOfLife = new GameOfLife.GameOfLife(width, height, generations);
            var result = gameOfLife.Run(token);
            return new JsonResult(result);
        }
    }
}
