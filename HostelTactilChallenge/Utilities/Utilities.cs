using HostelTactilChallenge.Models;

namespace HostelTactilChallenge;

public static class Utilities
{
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

    public static List<List<Chip>> HalfTranspose(List<List<Chip>> array)
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
