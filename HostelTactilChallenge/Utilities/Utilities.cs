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
}
