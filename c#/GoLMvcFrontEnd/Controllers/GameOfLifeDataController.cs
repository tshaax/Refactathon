using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoLMvcFrontEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameOfLifeDataController : ControllerBase
    {
        [HttpGet]
        [Route("GetAsBools")]
        public Task<JsonResult> GetAsBools(int width, int height, int generations,
            int percentageFilledAtStart = 40, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var gameOfLife = new GameOfLife.GameOfLife(width, height, generations, percentageFilledAtStart);
                var result = gameOfLife.Run(cancellationToken);
                return new JsonResult(result.ToList());
            }, cancellationToken);
        }

        [HttpGet]
        [Route("GetAsStrings")]
        public Task<JsonResult> GetAsStrings(int width, int height, int generations,
            int percentageFilledAtStart = 40, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var gameOfLife = new GameOfLife.GameOfLife(width, height, generations, percentageFilledAtStart);
                var result = gameOfLife.BoardsToString(gameOfLife.Run(cancellationToken));
                return new JsonResult(result.ToList());
            }, cancellationToken);
        }
    }
}
