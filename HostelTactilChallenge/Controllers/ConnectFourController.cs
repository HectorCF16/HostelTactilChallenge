using HostelTactilChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipelines;

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
                    if (c == Convert.ToChar(enumValue))
                    {
                        yield return enumValue;
                        break;
                    }
                }
            }
        }

        private Board ReadBoard(string board)
        {
            return new Board(Enumerable.Range(0, boardColumns).Select(i => new BoardColumn
            {
                Cells = GetMatchingEnumValues<Chip>(board.Substring(i * boardRows, boardRows))
            }).ToArray());
        }

        bool CheckBoardSize(string board)
        {
            return board.Length == boardColumns * boardRows;
        }

        bool CheckChipsByTeam(string board)
        {
            int teamAChips = board.Count(c => c == Convert.ToChar(Chip.TeamA));
            int teamBChips = board.Count(c => c == Convert.ToChar(Chip.TeamB));
            int difference = teamAChips - teamBChips;

            return difference > -1 && difference <= 1;
        }

        [HttpGet("{message}")]
        public Board Get(string message)
        {
            if (!CheckBoardSize(message))
                return new Board(Enumerable.Empty<BoardColumn>());

            if (!CheckChipsByTeam(message))
                return new Board(Enumerable.Empty<BoardColumn>());

            Board board = ReadBoard(message);

            return board;
        }
    }
}
