using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobotFactory.Models;
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
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "XM-1" }
            };

            _stockManager?.VerifyOrder(input);
            _mock?.Verify(m => m.VerifyOrder(input), Times.Once);
        }

        [TestMethod]
        public void Produce_ShouldCallMethodWithCorrectArgument()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 2, RobotName = "RD-1" }
            };

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
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "XM-1" }
            };

            _stockManager?.DisplayInstructions(input);
            _mock?.Verify(m => m.DisplayInstructions(input), Times.Once);
        }

        [TestMethod]
        public void Produce_ShouldPrintStockUpdated_WhenEnoughStock()
        {
            // Arrange
            var stockManager = new StockManager();
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "XM-1" }
            };
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            stockManager.Produce(input);
            var output = sw.ToString();

            // Assert
            bool containsStockUpdated = output.Contains("STOCK_UPDATED");
            Assert.IsTrue(containsStockUpdated, $"La sortie console attendue 'STOCK_UPDATED' est absente.\nSortie actuelle :\n{output}");
        }


    }
}