using RobotFactory.Services;
using RobotFactory.Services.Impl;
using RobotFactory.Utils;

namespace RobotFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRobotService robotService = new RobotService();
            IStockManager stockManager = new StockManager(robotService);
            IInstructionService instructionService = new InstructionService();
            IOrderService orderService = new OrderService(robotService, instructionService, stockManager);

            Console.WriteLine("Bienvenue dans RobotFactory !");
            Console.WriteLine("Entrez vos commandes (STOCKS, NEEDED_STOCKS, INSTRUCTIONS, VERIFY, PRODUCE)");
            Console.WriteLine("Format avancé disponible pour WITH/WITHOUT/REPLACE via ;");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string command = input.Split(' ')[0].ToUpper();
                string arguments = input.Length > command.Length ? input.Substring(command.Length).Trim() : "";

                switch (command)
                {
                    case "STOCKS":
                        stockManager.DisplayStocks();
                        break;

                    case "NEEDED_STOCKS":
                        var parsedSimple = Parser.ParseArguments(arguments);
                        if (parsedSimple.Count == 0)
                        {
                            Console.WriteLine("Format incorrect.");
                            continue;
                        }

                        stockManager.DisplayNeededPieces(parsedSimple);
                        break;

                    case "INSTRUCTIONS":
                        orderService.DisplayInstructions(Parser.ParseComplexArguments(arguments));
                        break;

                    case "VERIFY":
                        orderService.VerifyOrder(Parser.ParseComplexArguments(arguments));
                        break;

                    case "PRODUCE":
                        orderService.Produce(Parser.ParseComplexArguments(arguments));
                        break;

                    default:
                        Console.WriteLine("Commande inconnue !");
                        break;
                }
            }
        }
    }
}