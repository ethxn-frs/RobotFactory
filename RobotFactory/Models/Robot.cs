namespace RobotFactory.Models;

public class Robot
{
    public string Name { get; set; }
    public List<string> RequiredPieces { get; set; }

    public Robot(string name, List<string> requiredPieces)
    {
        Name = name;
        RequiredPieces = requiredPieces;
    }
}