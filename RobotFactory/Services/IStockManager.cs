namespace RobotFactory.Services
{
    public interface IStockManager
    {
        void DisplayInstructions(string arguments);
        void DisplayNeededPieces(string arguments);
        void DisplayStocks();
        void Produce(string arguments);
        void VerifyOrder(string arguments);
    }
}
