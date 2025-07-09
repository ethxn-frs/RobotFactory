namespace RobotFactory.Models;

public interface IRobot
{
    string Name { get; }
    string Category { get; }
    List<string> GetPieces();
}
