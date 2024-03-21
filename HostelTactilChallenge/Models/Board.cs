namespace HostelTactilChallenge.Models
{
    public class Board
    {
        public IEnumerable<BoardColumn> Columns { get; set; }
        
        public Board(IEnumerable<BoardColumn> columns) 
        {
            Columns = columns;
        }
    }
}
