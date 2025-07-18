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
#if DEBUG
            StockManager.ResetSingleton();
#endif
            _robotService = new RobotService();
            StockManager.Initialize(_robotService);
            _stockManager = StockManager.Instance;
            _instructionService = new InstructionService();
            _orderService = new OrderService(_robotService, _instructionService, _stockManager);
        }


        private string CaptureConsoleOutput(Action action)
        {
            var originalOut = Console.Out;
            var sw = new StringWriter();
            Console.SetOut(sw);

            action();

            Console.SetOut(originalOut);
            return sw.ToString();
        }

        [TestMethod]
        public void DisplayInstructions_ShouldPrintExpectedInstructions()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "XM-1" }
            };

            var output = CaptureConsoleOutput(() => { _orderService.DisplayInstructions(input); });

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

            var output = CaptureConsoleOutput(() => { _orderService.VerifyOrder(input); });

            Assert.IsTrue(output.Contains("AVAILABLE"));
        }

        [TestMethod]
        public void Produce_ShouldPrintStockUpdated_WhenEnoughStock()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "WI-1" }
            };

            var output = CaptureConsoleOutput(() => { _orderService.Produce(input); });

            Assert.IsTrue(output.Contains("STOCK_UPDATED"));
        }

        [TestMethod]
        public void Produce_ShouldPrintError_WhenStockIsInsufficient()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 999, RobotName = "WI-1" }
            };

            var output = CaptureConsoleOutput(() => { _orderService.Produce(input); });

            Assert.IsTrue(output.Contains("ERROR Not enough stock."));
        }

        [TestMethod]
        public void DisplayInstructions_ShouldPrintError_ForInvalidRobotName()
        {
            var input = new List<ParsedRobotOrder>
            {
                new ParsedRobotOrder { Quantity = 1, RobotName = "INVALID" }
            };

            var output = CaptureConsoleOutput(() => { _orderService.DisplayInstructions(input); });

            Assert.IsTrue(output.Contains("ERROR Robot inconnu"));
        }
    }
}