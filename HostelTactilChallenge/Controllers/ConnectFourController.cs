using HostelTactilChallenge.Models;
using Microsoft.AspNetCore.Mvc;

namespace HostelTactilChallenge.Controllers
{
    [ApiController]
    [Route("connect-four")]
    public class ConnectFourController : ControllerBase
    {
        private readonly int boardColumns = 7;
        private readonly int boardRows = 6;

        [HttpGet("{board}")]
        public IEnumerable<BoardColumn> Get(string board)
        {
            return Enumerable.Range(0, boardColumns).Select(index => new BoardColumn
            {
                Rows = board.Substring(index * boardRows, boardRows)
            })
            .ToArray();
        }
    }
}
