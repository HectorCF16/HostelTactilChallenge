using HostelTactilChallenge.Models;

namespace HostelTactilChallenge;

public static class Utilities
{

    // Check if sequence of chips exists ordered inside other sequence of chips
    public static bool SequenceExists(IEnumerable<Chip> enumerable, IEnumerable<Chip> sequence)
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

    // Rotate 45 degrees 2D List, so diagonals become rows and columns
    public static List<List<Chip>> Rotate45(List<List<Chip>> array)
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

    // Rotate Matrix 90 degrees to the right, so  the number of rows becomes the number of columns, and vice versa
    public static List<List<Chip>> Transpose(List<List<Chip>> board)
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
}
