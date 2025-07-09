using RobotFactory.Models;

namespace RobotFactory.Services.Impl
{
    public interface IOrderService
    {
        void DisplayInstructions(List<ParsedRobotOrder> orders);
        void VerifyOrder(List<ParsedRobotOrder> orders);
        void Produce(List<ParsedRobotOrder> orders);
    }
}