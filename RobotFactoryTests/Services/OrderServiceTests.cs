using RobotFactory.Models;
using RobotFactory.Services;
using RobotFactory.Services.Impl;

namespace RobotFactoryTests.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        private IRobotService _robotService;
        private IInstructionService _instructionService;
        private IStockManager _stockManager;
        private IOrderService _orderService;

        [TestInitialize]
        public void Setup()
        {
            _robotService = new RobotService();
            StockManager.Initialize(_robotService);
            _stockManager = StockManager.Instance;
            _instructionService = new InstructionService();
            _orderService = new OrderService(_robotService, _instructionService, _stockManager);
        }

        [TestMethod]
        public void DisplayInstructions_ShouldPrintExpectedInstructions()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "XM-1" }
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            _orderService.DisplayInstructions(input);
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("PRODUCING XM-1"));
            Assert.IsTrue(output.Contains("FINISHED XM-1"));
        }

        [TestMethod]
        public void VerifyOrder_ShouldPrintAvailable_WhenStockIsSufficient()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "RD-1" }
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            _orderService.VerifyOrder(input);
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("AVAILABLE"));
        }

        [TestMethod]
        public void Produce_ShouldPrintStockUpdated_WhenEnoughStock()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "WI-1" }
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            _orderService.Produce(input);
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("STOCK_UPDATED"));
        }

        [TestMethod]
        public void Produce_ShouldPrintError_WhenStockIsInsufficient()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 999, RobotName = "WI-1" }
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            _orderService.Produce(input);
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("ERROR Not enough stock."));
        }

        [TestMethod]
        public void DisplayInstructions_ShouldPrintError_ForInvalidRobotName()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "INVALID" }
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            _orderService.DisplayInstructions(input);
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("ERROR Robot inconnu"));
        }
    }
}