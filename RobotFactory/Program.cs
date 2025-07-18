using RobotFactory.Commands;
using RobotFactory.Services;
using RobotFactory.Services.Impl;

namespace RobotFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRobotService robotService = new RobotService();
            StockManager.Initialize(robotService);
            IStockManager stockManager = StockManager.Instance;
            IInstructionService instructionService = new InstructionService();
            IOrderService orderService = new OrderService(robotService, instructionService, stockManager);

            var commands = new List<ICommand>
            {
                new StocksCommand(stockManager),
                new NeededStocksCommand(stockManager),
                new InstructionsCommand(orderService),
                new VerifyCommand(orderService),
                new ProduceCommand(orderService),
                new AddTemplateCommand(robotService)
            };

            Console.WriteLine("Bienvenue dans RobotFactory !");
            Console.WriteLine(
                "Commandes disponibles : STOCKS, NEEDED_STOCKS, INSTRUCTIONS, VERIFY, PRODUCE, ADD_TEMPLATE");
            Console.WriteLine("Format avancé : WITH/WITHOUT/REPLACE via ;");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(input)) continue;

                var commandName = input.Split(' ')[0].ToUpper();
                var arguments = input.Length > commandName.Length ? input.Substring(commandName.Length).Trim() : "";

                var command = commands.FirstOrDefault(c => c.Name == commandName);
                if (command != null)
                    command.Execute(arguments);
                else
                    Console.WriteLine("Commande inconnue !");
            }
        }
    }
}