using RobotFactory.Services;
using RobotFactory.Services.Impl;

namespace RobotFactoryTests.Services
{
    [TestClass]
    public class StockManagerTests
    {
        private IStockManager? _stockManager;

        [TestInitialize]
        public void Setup()
        {
            IRobotService robotService = new RobotService();
            StockManager.Initialize(robotService);
            _stockManager = StockManager.Instance;
        }

        [TestMethod]
        public void DisplayStocks_ShouldPrintExpectedOutput()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            _stockManager!.DisplayStocks();
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("Robots disponibles"));
            Assert.IsTrue(output.Contains("Pièces disponibles"));
        }

        [TestMethod]
        public void DisplayNeededPieces_ShouldPrintCorrectLines()
        {
            var robots = new Dictionary<string, int> { { "WI-1", 1 } };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            _stockManager!.DisplayNeededPieces(robots);
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("Pièces nécessaires"), "Missing 'Pièces nécessaires' section.");
            Assert.IsTrue(output.Contains("WI-1"), "Missing robot name in output.");
        }

        [TestMethod]
        public void IsAvailable_ShouldReturnTrue_WhenStockIsSufficient()
        {
            var needed = new Dictionary<string, int>
            {
                { "Core_CM1", 1 },
                { "Generator_GM1", 1 },
                { "Arms_AM1", 1 },
                { "Legs_LM1", 1 },
                { "System_SB1", 1 }
            };

            var result = _stockManager!.IsAvailable(needed);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Consume_ShouldReduceStockCorrectly()
        {
            var needed = new Dictionary<string, int>
            {
                { "Core_CM1", 1 },
                { "Generator_GM1", 1 }
            };

            _stockManager!.Consume(needed);

            using var sw = new StringWriter();
            Console.SetOut(sw);
            _stockManager.DisplayStocks();
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("9 Core_CM1"));
            Assert.IsTrue(output.Contains("9 Generator_GM1"));
        }

        [TestMethod]
        public void AddRobots_ShouldIncreaseRobotStock()
        {
            _stockManager!.AddRobots("XM-1", 3);

            using var sw = new StringWriter();
            Console.SetOut(sw);
            _stockManager.DisplayStocks();
            var output = sw.ToString();

            Assert.IsTrue(output.Contains("3 XM-1"));
        }
    }
}