using RobotFactory.Models;
using RobotFactory.Services.Impl;

namespace RobotFactory.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRobotService _robotService;
        private readonly IInstructionService _instructionService;
        private readonly IStockManager _stockManager;

        public OrderService(IRobotService robotService, IInstructionService instructionService,
            IStockManager stockManager)
        {
            _robotService = robotService;
            _instructionService = instructionService;
            _stockManager = stockManager;
        }

        public void DisplayInstructions(List<ParsedRobotOrder> orders)
        {
            foreach (var order in orders)
            {
                Robot robot;
                try
                {
                    robot = _robotService.GetBaseRobot(order.RobotName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR {ex.Message}");
                    continue;
                }

                var pieces = _robotService.GetModifiedPieces(order);
                if (!_robotService.ValidateCategories(robot, pieces, out var err))
                {
                    Console.WriteLine($"ERROR {err}");
                    continue;
                }

                for (int i = 0; i < order.Quantity; i++)
                {
                    var instructions = _instructionService.GenerateInstructions(robot.Name, pieces);
                    foreach (var line in instructions)
                        Console.WriteLine(line);
                }
            }
        }

        public void VerifyOrder(List<ParsedRobotOrder> orders)
        {
            var needed = new Dictionary<string, int>();

            foreach (var order in orders)
            {
                Robot robot;
                try
                {
                    robot = _robotService.GetBaseRobot(order.RobotName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR {ex.Message}");
                    return;
                }

                var pieces = _robotService.GetModifiedPieces(order);
                if (!_robotService.ValidateCategories(robot, pieces, out var err))
                {
                    Console.WriteLine($"ERROR {err}");
                    return;
                }

                foreach (var p in pieces)
                    if (needed.ContainsKey(p)) needed[p] += order.Quantity;
                    else needed[p] = order.Quantity;
            }

            if (_stockManager.IsAvailable(needed)) Console.WriteLine("AVAILABLE");
            else Console.WriteLine("UNAVAILABLE");
        }

        public void Produce(List<ParsedRobotOrder> orders)
        {
            var needed = new Dictionary<string, int>();

            foreach (var order in orders)
            {
                Robot robot;
                try
                {
                    robot = _robotService.GetBaseRobot(order.RobotName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR {ex.Message}");
                    return;
                }

                var pieces = _robotService.GetModifiedPieces(order);
                if (!_robotService.ValidateCategories(robot, pieces, out var err))
                {
                    Console.WriteLine($"ERROR {err}");
                    return;
                }

                foreach (var p in pieces)
                    if (needed.ContainsKey(p)) needed[p] += order.Quantity;
                    else needed[p] = order.Quantity;
            }

            if (!_stockManager.IsAvailable(needed))
            {
                Console.WriteLine("ERROR Not enough stock.");
                return;
            }

            _stockManager.Consume(needed);
            foreach (var order in orders)
                _stockManager.AddRobots(order.RobotName, order.Quantity);

            Console.WriteLine("STOCK_UPDATED");
        }
    }
}