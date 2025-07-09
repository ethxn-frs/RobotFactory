namespace RobotFactory.Models
{
    public class ParsedRobotOrder
    {
        public int Quantity { get; set; }
        public string RobotName { get; set; } = "";
        public List<(int Quantity, string Piece)> WithPieces { get; set; } = new();
        public List<(int Quantity, string Piece)> WithoutPieces { get; set; } = new();
        public List<(int Quantity, string FromPiece, string ToPiece)> ReplacePieces { get; set; } = new();
    }
}