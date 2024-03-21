using HostelTactilChallenge;
using Xunit;
using System;
using HostelTactilChallenge.Models;
using static HostelTactilChallenge.Utilities;
using System.Collections.Generic;


namespace TestHostelTactilChallenge
{
    public class TestUtilities
    {
        

        public TestUtilities()
        {
        }

        [Fact]
        public void TestSequenceExistsTrue()
        {
            IEnumerable<Chip> chips4 = Enumerable.Repeat(Chip.TeamB, 4);
            IEnumerable<Chip> chips3 = Enumerable.Repeat(Chip.TeamB, 3);

            Assert.True(Utilities.SequenceExists(chips4, chips3));
        }

        [Fact]
        public void TestSequenceExistsFalse()
        {
            IEnumerable<Chip> chips4 = Enumerable.Repeat(Chip.TeamB, 4);
            IEnumerable<Chip> chips3 = Enumerable.Repeat(Chip.TeamA, 3);

            Assert.False(Utilities.SequenceExists(chips4, chips3));
        }
    }
}