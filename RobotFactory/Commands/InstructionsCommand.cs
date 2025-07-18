using RobotFactory.Services.Impl;
using RobotFactory.Utils;

namespace RobotFactory.Commands
{
    public class InstructionsCommand : ICommand
    {
        private readonly IOrderService _orderService;
        public string Name => "INSTRUCTIONS";

        public InstructionsCommand(IOrderService orderService) => _orderService = orderService;

        public void Execute(string arguments)
        {
            var parsed = Parser.ParseComplexArguments(arguments);
            _orderService.DisplayInstructions(parsed);
        }
    }
}