namespace RobotFactory.Models;

public class BasicRobotWrapper : IRobot
{
    private readonly Robot _robot;

    public BasicRobotWrapper(Robot robot)
    {
        _robot = robot;
    }

    public string Name => _robot.Name;
    public string Category => _robot.Category;

    public List<string> GetPieces()
    {
        return new List<string>(_robot.RequiredPieces);
    }
}
