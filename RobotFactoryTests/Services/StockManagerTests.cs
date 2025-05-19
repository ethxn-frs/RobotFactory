using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobotFactory.Services;
using System.Collections.Generic;

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
            var input = new Dictionary<string, int> { { "XM-1", 1 } };
            _stockManager?.VerifyOrder(input);
            _mock?.Verify(m => m.VerifyOrder(input), Times.Once);
        }

        [TestMethod]
        public void Produce_ShouldCallMethodWithCorrectArgument()
        {
            var input = new Dictionary<string, int> { { "RD-1", 2 } };
            _stockManager?.Produce(input);
            _mock?.Verify(m => m.Produce(input), Times.Once);
        }

        [TestMethod]
        public void DisplayNeededPieces_ShouldCallMethodWithCorrectArgument()
        {
            var input = new Dictionary<string, int> { { "WI-1", 3 } };
            _stockManager?.DisplayNeededPieces(input);
            _mock?.Verify(m => m.DisplayNeededPieces(input), Times.Once);
        }

        [TestMethod]
        public void DisplayInstructions_ShouldCallMethodWithCorrectArgument()
        {
            var input = new Dictionary<string, int> { { "XM-1", 1 } };
            _stockManager?.DisplayInstructions(input);
            _mock?.Verify(m => m.DisplayInstructions(input), Times.Once);
        }
    }
}
