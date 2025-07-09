using RobotFactory.Models;

namespace RobotFactory.Services
{
    public interface IStockManager
    {
        void DisplayInstructions(List<ParsedRobotOrder> robotOrders);
        void DisplayNeededPieces(Dictionary<string, int> requestedRobots);
        void DisplayStocks();
        void Produce(List<ParsedRobotOrder> robotOrders);
        void VerifyOrder(List<ParsedRobotOrder> robotOrders);
    }
}