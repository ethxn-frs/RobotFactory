using RobotFactory.Services.Impl;
using RobotFactory.Utils;

namespace RobotFactory.Commands
{
    public class VerifyCommand : ICommand
    {
        private readonly IOrderService _orderService;
        public string Name => "VERIFY";

        public VerifyCommand(IOrderService orderService) => _orderService = orderService;

        public void Execute(string arguments)
        {
            var parsed = Parser.ParseComplexArguments(arguments);
            _orderService.VerifyOrder(parsed);
        }
    }
}