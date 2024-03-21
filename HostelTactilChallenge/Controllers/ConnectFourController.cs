using HostelTactilChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using HostelTactilChallenge;
using System.IO.Pipelines;

namespace HostelTactilChallenge.Controllers
{
    [ApiController]
    [Route("connect-four")]
    public class ConnectFourController : ControllerBase
    {
        private readonly int boardColumns = 7;
        private readonly int boardRows = 6;

        private readonly Dictionary<Chip, Result> chipsTeams = new Dictionary<Chip, Result>()
        {
            { Chip.TeamA, Result.TeamAWins },
            { Chip.TeamB, Result.TeamBWins },
        };

        #region validateInputFunctions

        // Check size of the board is the expected
        private bool CheckBoardSize(string board)
        {
            return board.Length == boardColumns * boardRows;
        }

        // Check if TeamA is 1 or 0 chips more than TeamB
        private bool CheckChipsByTeam(string board)
        {
            int teamAChips = board.Count(c => c == Convert.ToChar(Chip.TeamA));
            int teamBChips = board.Count(c => c == Convert.ToChar(Chip.TeamB));
            int difference = teamAChips - teamBChips;

            return difference > -1 && difference <= 1;
        }

        // Check if any piece is floating
        private bool HasFloatingPieces(Board board)
        {
            // Iterate through each column
            foreach (BoardColumn boardColumn in board.Columns)
            {
                Chip[] floatingSequenceTeamA = { Chip.Empty, Chip.TeamA };
                Chip[] floatingSequenceTeamB = { Chip.Empty, Chip.TeamB };

                IEnumerable<Chip> floatingSequenceA = floatingSequenceTeamA;
                IEnumerable<Chip> floatingSequenceB = floatingSequenceTeamB;

                bool floatingASequence =Utilities.SequenceExists(boardColumn.Cells, floatingSequenceA);
                bool floatingBSequence =Utilities.SequenceExists(boardColumn.Cells, floatingSequenceB);

                if (floatingASequence || floatingBSequence)
                    return true;
            }
            // No floating pieces found
            return false; 
        }
        #endregion

        // Get Chip value from string board columns
        private IEnumerable<Chip> GetMatchingEnumValues<Chip>(string inputString)
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

        // Get Chip value string from board
        private Board ReadBoard(string board)
        {
            return new Board(Enumerable.Range(0, boardColumns).Select(i => new BoardColumn
            {
                Cells = GetMatchingEnumValues<Chip>(board.Substring(i * boardRows, boardRows))
            }).ToArray());
        }
        
        // Check if there's a sequence of repeated chips in a sequence of chips, given a type of chip
        private bool CheckLine(IEnumerable<Chip> chips, Chip chip, int length)
        {
            IEnumerable<Chip> line = Enumerable.Repeat(chip, length);

            return Utilities.SequenceExists(chips, line);
        }

        // Check if there's a sequence of 4 repeated chips in a column, given a type of chip
        private void CheckHorizontalLines(ref Result result, BoardColumn boardColumn)
        {
            foreach (var chipTeam in chipsTeams)
            {
                if (CheckLine(boardColumn.Cells, chipTeam.Key, 4))
                {
                    if (result == Result.None && !CheckLine(boardColumn.Cells, Chip.TeamA, 5))
                        result = chipTeam.Value;
                    else result = Result.IllegalMultipleLines;
                }
            }
        }

        // Check if there're connected chips in a sequence of chips for every type of chip
        private void CheckLines(ref Result result, List<Chip> row)
        {
            foreach (var chipTeam in chipsTeams)
            {
                if (CheckLine(row, chipTeam.Key, 4))
                {
                    if (result == Result.None)
                        result = chipTeam.Value;
                    else result = Result.IllegalMultipleLines;
                }
            }
        }

        // Check for connected chips in all lines of the boad
        private Result CheckAllLines(Board board)
        {
            Result result = Result.None;

            // Transform board to Lists
            List<List<Chip>> boardList = new List<List<Chip>>();

            foreach (var column in board.Columns)
            {
                var line = column.Cells.ToList();
                boardList.Add(line);
            }

            var transposedBoard = Utilities.Transpose(boardList);

            // Check vertical lines
            foreach(List<Chip> row in transposedBoard)
            {
                CheckLines(ref result, row);
                if (Result.IllegalMultipleLines == result)
                    return Result.IllegalMultipleLines;
            }

            // Check diagonal lines
            var rotated45 = Utilities.Rotate45(boardList);

            foreach (List<Chip> row in rotated45)
            {
                CheckLines(ref result, row);
                if (Result.IllegalMultipleLines == result)
                    return Result.IllegalMultipleLines;
            }

            foreach (BoardColumn boardColumn in board.Columns)
            {
                // Check horizontal lines
                CheckHorizontalLines(ref result, boardColumn);
                if (Result.IllegalMultipleLines == result)
                    return Result.IllegalMultipleLines;
            }
            return result;
        }

        // Error handling function
        private Result HandleBoardMessage(string message)
        {
            #region validateInput
            if (!CheckBoardSize(message))
                return Result.IllegalBoardSize;

            if (!CheckChipsByTeam(message))
                return Result.IllegalManyTeamChips;
            #endregion
            Board board = ReadBoard(message);

            if (HasFloatingPieces(board))
                return Result.IllegalFloatingChips;


            return CheckAllLines(board);
        }

        [HttpGet("{message}")]
        public string Get(string message)
        {
            switch (HandleBoardMessage(message))
            {
                case Result.TeamAWins:
                    return "A";
                case Result.TeamBWins:
                    return "B";
                default:
                    return "X";
            }
        }
    }
}
