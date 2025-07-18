using RobotFactory.Models;

namespace RobotFactory.Services.Impl
{
    public interface IStockManager
    {
        void DisplayNeededPieces(Dictionary<string, int> requestedRobots);
        void DisplayStocks();
        bool IsAvailable(Dictionary<string, int> needed);
        void Consume(Dictionary<string, int> needed);
        void AddRobots(string robotName, int qty);
        void ResetStock();
    }
}