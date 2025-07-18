using RobotFactory.Models;
using RobotFactory.Models.Decorators;

namespace RobotFactoryTests.Models.Decorators
{
    [TestClass]
    public class RobotServiceDecoratorsTests
    {
        private Robot _baseRobot;

        [TestInitialize]
        public void Setup()
        {
            _baseRobot = new Robot(
                name: "TestBot",
                requiredPieces: new List<string>
                {
                    "Core_CM1", "Generator_GM1", "Arms_AM1", "Legs_LM1", "System_SM1"
                },
                category: "M"
            );
        }

        [TestMethod]
        public void WithPiecesDecorator_ShouldAddPieces()
        {
            var robot = new BasicRobotWrapper(_baseRobot);
            var withDecorator = new WithPiecesDecorator(robot, new List<string> { "Extra_Piece1", "Extra_Piece2" });

            var pieces = withDecorator.GetPieces();

            Assert.IsTrue(pieces.Contains("Extra_Piece1"));
            Assert.IsTrue(pieces.Contains("Extra_Piece2"));
            Assert.IsTrue(pieces.Contains("Core_CM1"));
        }

        [TestMethod]
        public void WithoutPiecesDecorator_ShouldRemoveSpecifiedPieces()
        {
            var robot = new BasicRobotWrapper(_baseRobot);
            var withoutDecorator = new WithoutPiecesDecorator(robot, new List<string> { "Legs_LM1", "Arms_AM1" });

            var pieces = withoutDecorator.GetPieces();

            Assert.IsFalse(pieces.Contains("Legs_LM1"));
            Assert.IsFalse(pieces.Contains("Arms_AM1"));
            Assert.IsTrue(pieces.Contains("Core_CM1"));
        }

        [TestMethod]
        public void ReplacePiecesDecorator_ShouldSwapCorrectly()
        {
            var robot = new BasicRobotWrapper(_baseRobot);
            var replaceDecorator = new ReplacePiecesDecorator(robot, new List<(string, string)>
            {
                ("System_SM1", "System_SB1"),
                ("Arms_AM1", "Arms_AI1")
            });

            var pieces = replaceDecorator.GetPieces();

            Assert.IsFalse(pieces.Contains("System_SM1"));
            Assert.IsFalse(pieces.Contains("Arms_AM1"));
            Assert.IsTrue(pieces.Contains("System_SB1"));
            Assert.IsTrue(pieces.Contains("Arms_AI1"));
        }

        [TestMethod]
        public void CombineMultipleDecorators_ShouldWorkCorrectly()
        {
            IRobot robot = new BasicRobotWrapper(_baseRobot);
            robot = new WithoutPiecesDecorator(robot, new List<string> { "Generator_GM1" });
            robot = new ReplacePiecesDecorator(robot, new List<(string, string)>
            {
                ("Arms_AM1", "Arms_AI1")
            });
            robot = new WithPiecesDecorator(robot, new List<string> { "Extra_PieceX" });

            var pieces = robot.GetPieces();

            Assert.IsFalse(pieces.Contains("Generator_GM1"));
            Assert.IsFalse(pieces.Contains("Arms_AM1"));
            Assert.IsTrue(pieces.Contains("Arms_AI1"));
            Assert.IsTrue(pieces.Contains("Extra_PieceX"));
        }
    }
}