using RobotFactory.Models;
using RobotFactory.Services;

namespace RobotFactoryTests.Services
{
    [TestClass]
    public class RobotServiceTests
    {
        private RobotService _robotService;

        [TestInitialize]
        public void Setup()
        {
            _robotService = new RobotService();
        }

        [TestMethod]
        public void GetBaseRobot_ShouldReturnRobot_WhenNameIsValid()
        {
            var robot = _robotService.GetBaseRobot("XM-1");

            Assert.IsNotNull(robot);
            Assert.AreEqual("XM-1", robot.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBaseRobot_ShouldThrowException_WhenNameIsInvalid()
        {
            _robotService.GetBaseRobot("INVALID_ROBOT");
        }

        [TestMethod]
        public void GetModifiedPieces_ShouldInclude_WithPieces()
        {
            var order = new ParsedRobotOrder
            {
                RobotName = "RD-1",
                WithPieces = new List<(int, string)>
                {
                    (1, "Arms_AI1")
                }
            };

            var pieces = _robotService.GetModifiedPieces(order);
            Assert.IsTrue(pieces.Contains("Arms_AI1"));
        }

        [TestMethod]
        public void GetModifiedPieces_ShouldExclude_WithoutPieces()
        {
            var order = new ParsedRobotOrder
            {
                RobotName = "RD-1",
                WithoutPieces = new List<(int, string)>
                {
                    (1, "Arms_AD1")
                }
            };

            var pieces = _robotService.GetModifiedPieces(order);
            Assert.IsFalse(pieces.Contains("Arms_AD1"));
        }

        [TestMethod]
        public void GetModifiedPieces_ShouldReplace_PiecesCorrectly()
        {
            var order = new ParsedRobotOrder
            {
                RobotName = "RD-1",
                ReplacePieces = new List<(int, string, string)>
                {
                    (1, "Arms_AD1", "Arms_AI1")
                }
            };

            var pieces = _robotService.GetModifiedPieces(order);
            Assert.IsFalse(pieces.Contains("Arms_AD1"));
            Assert.IsTrue(pieces.Contains("Arms_AI1"));
        }

        [TestMethod]
        public void ValidateCategories_ShouldReturnTrue_WhenAllCompatible()
        {
            var robot = _robotService.GetBaseRobot("RD-1");
            var pieces = new List<string> { "Core_CD1", "Generator_GD1", "Arms_AD1", "Legs_LD1", "System_SB1" };

            var result = _robotService.ValidateCategories(robot, pieces, out var error);

            Assert.IsTrue(result);
            Assert.AreEqual("", error);
        }

        [TestMethod]
        public void ValidateCategories_ShouldReturnFalse_WhenIncompatiblePiece()
        {
            var robot = _robotService.GetBaseRobot("RD-1");
            var pieces = new List<string> { "Core_CM1" };

            var result = _robotService.ValidateCategories(robot, pieces, out var error);

            Assert.IsFalse(result);
            Assert.IsTrue(error.Contains("incompatible"));
        }
    }
}