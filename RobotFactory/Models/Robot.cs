namespace RobotFactory.Models;

public class Robot
{
    public string Name { get; set; }
    public List<string> RequiredPieces { get; set; }
    public string Category { get; set; }

    public Robot(string name, List<string> requiredPieces, string category)
    {
        Name = name;
        RequiredPieces = requiredPieces;
        Category = category;
    }
}