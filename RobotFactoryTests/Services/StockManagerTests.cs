using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobotFactory.Services;

namespace RobotFactoryTests.Services
{
    [TestClass]
    public class StockManagerTests
    {
        private Mock<IStockManager>? _mock;
        private IStockManager? _stockManager;

        [TestInitialize]
        public void Setup()
        {
            _mock = new Mock<IStockManager>();
            _stockManager = _mock.Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mock?.Reset();
        }

        [TestMethod]
        public void DisplayStocks_ShouldCallMethod()
        {
            _stockManager?.DisplayStocks();
            _mock?.Verify(m => m.DisplayStocks(), Times.Once);
        }

        [TestMethod]
        public void VerifyOrder_ShouldCallMethodWithCorrectArgument()
        {
            var input = "1 XM-1";
            _stockManager?.VerifyOrder(input);
            _mock?.Verify(m => m.VerifyOrder("1 XM-1"), Times.Once);
        }

        [TestMethod]
        public void Produce_ShouldCallMethodWithCorrectArgument()
        {
            var input = "2 RD-1";
            _stockManager?.Produce(input);
            _mock?.Verify(m => m.Produce("2 RD-1"), Times.Once);
        }

        [TestMethod]
        public void DisplayNeededPieces_ShouldCallMethodWithCorrectArgument()
        {
            var input = "3 WI-1";
            _stockManager?.DisplayNeededPieces(input);
            _mock?.Verify(m => m.DisplayNeededPieces("3 WI-1"), Times.Once);
        }

        [TestMethod]
        public void DisplayInstructions_ShouldCallMethodWithCorrectArgument()
        {
            var input = "1 XM-1";
            _stockManager?.DisplayInstructions(input);
            _mock?.Verify(m => m.DisplayInstructions("1 XM-1"), Times.Once);
        }
    }
}
