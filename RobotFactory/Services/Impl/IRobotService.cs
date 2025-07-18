using RobotFactory.Models;

namespace RobotFactory.Services.Impl
{
    public interface IRobotService
    {
        Robot GetBaseRobot(string robotName);
        List<string> GetModifiedPieces(ParsedRobotOrder order);
        bool ValidateCategories(Robot robot, List<string> pieces, out string error);
        public void AddTemplate(string name, List<string> pieces);
    }
}