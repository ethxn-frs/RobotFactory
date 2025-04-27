using RobotFactory.Services;

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

                string command = input.Split(' ')[0];
                string arguments = input.Length > command.Length ? input.Substring(command.Length).Trim() : "";

                switch (command.ToUpper())
                {
                    case "STOCKS":
                        stockManager.DisplayStocks();
                        break;
                    case "NEEDED_STOCKS":
                        stockManager.DisplayNeededPieces(arguments);
                        break;
                    case "INSTRUCTIONS":
                        stockManager.DisplayInstructions(arguments);
                        break;
                    case "VERIFY":
                        stockManager.VerifyOrder(arguments);
                        break;
                    case "PRODUCE":
                        stockManager.Produce(arguments);
                        break;
                    default:
                        Console.WriteLine("Commande inconnue !");
                        break;
                }
            }
        }
    }
}