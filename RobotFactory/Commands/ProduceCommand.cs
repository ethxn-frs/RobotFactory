using RobotFactory.Services.Impl;
using RobotFactory.Utils;

namespace RobotFactory.Commands
{
    public class ProduceCommand : ICommand
    {
        private readonly IOrderService _orderService;
        public string Name => "PRODUCE";

        public ProduceCommand(IOrderService orderService) => _orderService = orderService;

        public void Execute(string arguments)
        {
            var parsed = Parser.ParseComplexArguments(arguments);
            _orderService.Produce(parsed);
        }
    }
}