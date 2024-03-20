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

        static IEnumerable<Chip> GetMatchingEnumValues<Chip>(string inputString) where Chip : struct, Enum
        {
            foreach (char c in inputString)
            {
                foreach (Chip enumValue in Enum.GetValues(typeof(Chip)))
                {
                    if (c.ToString().Equals(enumValue.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        yield return enumValue;
                        break;
                    }
                }
            }
        }

        [HttpGet("{board}")]
        public IEnumerable<BoardColumn> Get(string board)
        {
            return Enumerable.Range(0, boardColumns).Select(i => new BoardColumn
            {
                Rows = GetMatchingEnumValues<Chip>(board.Substring(i * boardRows, boardRows))
            }).ToArray();
        }
    }
}
