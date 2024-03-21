using HostelTactilChallenge;
using Xunit;
using System;
using HostelTactilChallenge.Controllers;


namespace TestHostelTactilChallenge
{
    public class TestConnectFourApi
    {
        private readonly ConnectFourController _controller;

        public TestConnectFourApi()
        {
            _controller = new ConnectFourController();
        }

        [Fact]
        public void GetNone()
        {
            string message = "AAXXXXBBXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "X");
        }

        [Fact]
        public void GetTeamAWins()
        {
            string message = "AAAAXXBBBXXXBXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "A");
        }

        [Fact]
        public void GetTeamBWins()
        {
            string message = "AAXXXXBBBBXXAAXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "B");
        }

        [Fact]
        public void GetTeamBWinsHorizontally()
        {
            string message = "BAXXXXBAXXXXBAXXXXBXXXXXAXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "B");
        }

        [Fact]
        public void GetTeamBWinsDiagonally()
        {
            string message = "BAXXXXABXXXXAABXXXABABXXXXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "B");
        }

        [Fact]
        public void GetFloatingChips()
        {
            string message = "AAXXXXBBXAXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "X");
        }

        [Fact]
        public void GetManyChips()
        {
            string message = "AAXXXXBBAAXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "X");
        }

        [Fact]
        public void GetIllegalBoardSize()
        {
            string message = "AAXXXXBBXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            var result = _controller.Get(message);
            Assert.Equal(result, "X");
        }
    }
}