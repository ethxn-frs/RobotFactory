using RobotFactory.Models;

namespace RobotFactory.Services.Impl
{
    public interface IStockManager
    {
        void DisplayInstructions(List<ParsedRobotOrder> robotOrders);
        void DisplayNeededPieces(Dictionary<string, int> requestedRobots);
        void DisplayStocks();
        void Produce(List<ParsedRobotOrder> robotOrders);
        void VerifyOrder(List<ParsedRobotOrder> robotOrders);
        bool IsAvailable(Dictionary<string, int> needed);
        void Consume(Dictionary<string, int> needed);
        void AddRobots(string robotName, int qty);

    }
}