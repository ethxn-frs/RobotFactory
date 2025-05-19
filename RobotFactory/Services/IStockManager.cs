namespace RobotFactory.Services
{
    public interface IStockManager
    {
        void DisplayInstructions(Dictionary<string, int> requestedRobots);
        void DisplayNeededPieces(Dictionary<string, int> requestedRobots);
        void DisplayStocks();
        void Produce(Dictionary<string, int> requestedRobots);
        void VerifyOrder(Dictionary<string, int> requestedRobots);
    }
}
