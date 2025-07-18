using RobotFactory.Services.Impl;

namespace RobotFactory.Commands
{
    public class StocksCommand : ICommand
    {
        private readonly IStockManager _stockManager;
        public string Name => "STOCKS";

        public StocksCommand(IStockManager stockManager) => _stockManager = stockManager;

        public void Execute(string arguments) => _stockManager.DisplayStocks();
    }
}