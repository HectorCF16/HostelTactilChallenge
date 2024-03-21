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

        static bool SequenceExists(IEnumerable<Chip> enumerable, IEnumerable<Chip> sequence)
        {
            for (int i = 0; i <= enumerable.Count() - sequence.Count(); i++)
            {
                if (enumerable.Skip(i).Take(sequence.Count()).SequenceEqual(sequence))
                {
                    return true;
                }
            }
            return false;
        }

        bool HasFloatingPieces(Board board)
        {
            // Iterate through each column
            foreach (BoardColumn boardColumn in board.Columns)
            {
                Chip[] floatingSequenceTeamA = { Chip.Empty, Chip.TeamA };
                Chip[] floatingSequenceTeamB = { Chip.Empty, Chip.TeamB };

                IEnumerable<Chip> floatingSequenceA = floatingSequenceTeamA;
                IEnumerable<Chip> floatingSequenceB = floatingSequenceTeamB;

                bool floatingASequence = SequenceExists(boardColumn.Cells, floatingSequenceA);
                bool floatingBSequence = SequenceExists(boardColumn.Cells, floatingSequenceB);

                if (floatingASequence || floatingBSequence)
                    return true;
            }

            return false; // No floating pieces found
        }

        static List<List<Chip>> HalfTranspose(List<List<Chip>> array)
        {
            int rows = array.Count;
            int columns = array[0].Count;
            int maxLength = Math.Max(rows, columns);

            List<List<Chip>> halfTransposedArray = new List<List<Chip>>();

            for (int i = 0; i < maxLength; i++)
            {
                halfTransposedArray.Add(new List<Chip>());

                for (int j = 0; j < maxLength; j++)
                {
                    int row = j;
                    int column = i + j;

                    if (column < columns && row < rows)
                    {
                        halfTransposedArray[i].Add(array[row][column]);
                    }
                }
            }

            return halfTransposedArray;
        }

        bool CheckLine(IEnumerable<Chip> chips, Chip chip, int length)
        {
            IEnumerable<Chip> line = Enumerable.Repeat(chip, length);

            return SequenceExists(chips, line);
        }

        Result CheckAllLines(Board board)
        {
            Result result = Result.None;
            List<List<Chip>> boardLines = new List<List<Chip>>();

            foreach (var column in board.Columns)
            {
                var line = column.Cells.ToList();
                boardLines.Add(line);
            }

            var transposedBoard = Transpose(boardLines);

            //check horizontal wins
            foreach(List<Chip> row in transposedBoard)
            {
                if (CheckLine(row, Chip.TeamA, 4))
                {
                    if (result == Result.None)
                        result = Result.TeamAWins;
                    else return Result.IllegalPosition;
                }

                if (CheckLine(row, Chip.TeamB, 4))
                {
                    if (result == Result.None)
                        result = Result.TeamBWins;
                    else return Result.IllegalPosition;
                }
            }

            //check diagonals
            var halfTransposed = HalfTranspose(boardLines);

            foreach (List<Chip> row in halfTransposed)
            {
                if (CheckLine(row, Chip.TeamA, 4))
                {
                    if (result == Result.None)
                        result = Result.TeamAWins;
                    else return Result.IllegalPosition;
                }

                if (CheckLine(row, Chip.TeamB, 4))
                {
                    if (result == Result.None)
                        result = Result.TeamBWins;
                    else return Result.IllegalPosition;
                }
            }

            foreach (BoardColumn boardColumn in board.Columns)
            {
                // Check vertical lines
                //para deshacerse de estos dos ifs habria que primero relacionar el Result.TeamAWins con las Chip.TeamA y con el teamB
                if(CheckLine(boardColumn.Cells, Chip.TeamA, 4))
                {
                    if (result == Result.None && !CheckLine(boardColumn.Cells, Chip.TeamA, 5))
                        result = Result.TeamAWins;
                    else return Result.IllegalPosition;
                }

                if(CheckLine(boardColumn.Cells, Chip.TeamB, 4))
                {
                    if (result == Result.None && !CheckLine(boardColumn.Cells, Chip.TeamB, 5))
                        result = Result.TeamBWins;
                    else return Result.IllegalPosition;
                }
            }


            return result;
        }

        static List<List<Chip>> Transpose(List<List<Chip>> board)
        {
            int rows = board.Count;
            int columns = board[0].Count;

            List<List<Chip>> transposedBoard = new List<List<Chip>>();

            for (int j = 0; j < columns; j++)
            {
                List<Chip> column = new List<Chip>();
                for (int i = 0; i < rows; i++)
                {
                    column.Add(board[i][j]);
                }
                transposedBoard.Add(column);
            }

            return transposedBoard;
        }


        [HttpGet("{message}")]
        public string Get(string message)
        {
            if (!CheckBoardSize(message))
                return Result.IllegalPosition.ToString();

            if (!CheckChipsByTeam(message))
                return Result.IllegalPosition.ToString();
            Board board = ReadBoard(message);

            if (HasFloatingPieces(board))
                return Result.IllegalPosition.ToString();


            return CheckAllLines(board).ToString();
        }
    }
}
