using RobotFactory.Services.Impl;
using RobotFactory.Utils;

namespace RobotFactory.Commands
{
    public class NeededStocksCommand : ICommand
    {
        private readonly IStockManager _stockManager;
        public string Name => "NEEDED_STOCKS";

        public NeededStocksCommand(IStockManager stockManager) => _stockManager = stockManager;

        public void Execute(string arguments)
        {
            var parsed = Parser.ParseArguments(arguments);
            if (parsed.Count == 0)
            {
                Console.WriteLine("Format incorrect.");
                return;
            }

            _stockManager.DisplayNeededPieces(parsed);
        }
    }
}