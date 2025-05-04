using RobotFactory.Services;
using RobotFactory.Utils;

namespace RobotFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stockManager = new StockManager();

            Console.WriteLine("Bienvenue dans RobotFactory !");
            Console.WriteLine("Entrez vos commandes (STOCKS, NEEDED_STOCKS, INSTRUCTIONS, VERIFY, PRODUCE)");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string command = input.Split(' ')[0].ToUpper();
                string arguments = input.Length > command.Length ? input.Substring(command.Length).Trim() : "";
                var parsedArguments = Parser.ParseArguments(arguments);

                if(parsedArguments.Count == 0 && command != "STOCKS") {
                    continue;
                }

                switch (command)
                {
                    case "STOCKS":
                        stockManager.DisplayStocks();
                        break;
                    case "NEEDED_STOCKS":
                        stockManager.DisplayNeededPieces(parsedArguments);
                        break;
                    case "INSTRUCTIONS":
                        stockManager.DisplayInstructions(parsedArguments);
                        break;
                    case "VERIFY":
                        stockManager.VerifyOrder(parsedArguments);
                        break;
                    case "PRODUCE":
                        stockManager.Produce(parsedArguments);
                        break;
                    default:
                        Console.WriteLine("Commande inconnue !");
                        break;
                }
            }
        }
    }
}