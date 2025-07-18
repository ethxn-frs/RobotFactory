using RobotFactory.Utils;

namespace RobotFactoryTests.Utils
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseArguments_ShouldParseSimpleFormat()
        {
            var input = "2 XM-1, 1 RD-1";
            var result = Parser.ParseArguments(input);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result["XM-1"]);
            Assert.AreEqual(1, result["RD-1"]);
        }

        [TestMethod]
        public void ParseComplexArguments_ShouldParseValidInput()
        {
            var input =
                "1 XM-1 WITH 1 Arms_AI1, 1 Legs_LI1 WITHOUT 1 Arms_AM1 REPLACE 1 Generator_GM1, 1 Generator_GI1";
            var result = Parser.ParseComplexArguments(input);

            Assert.AreEqual(1, result.Count);
            var order = result[0];

            Assert.AreEqual("XM-1", order.RobotName);
            Assert.AreEqual(1, order.Quantity);

            Assert.AreEqual(2, order.WithPieces.Count);
            Assert.IsTrue(order.WithPieces.Exists(p => p.Piece == "Arms_AI1"));
            Assert.IsTrue(order.WithPieces.Exists(p => p.Piece == "Legs_LI1"));

            Assert.AreEqual(1, order.WithoutPieces.Count);
            Assert.AreEqual("Arms_AM1", order.WithoutPieces[0].Piece);

            Assert.AreEqual(1, order.ReplacePieces.Count);
            var (qty, from, to) = order.ReplacePieces[0];
            Assert.AreEqual(1, qty);
            Assert.AreEqual("Generator_GM1", from);
            Assert.AreEqual("Generator_GI1", to);
        }

        [TestMethod]
        public void ParseComplexArguments_ShouldReturnEmptyList_OnEmptyInput()
        {
            var input = "";
            var result = Parser.ParseComplexArguments(input);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ParseComplexArguments_ShouldHandleMultipleOrders()
        {
            var input = "1 XM-1; 2 RD-1 WITH 1 Arms_AD1";
            var result = Parser.ParseComplexArguments(input);

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual("XM-1", result[0].RobotName);
            Assert.AreEqual(1, result[0].Quantity);

            Assert.AreEqual("RD-1", result[1].RobotName);
            Assert.AreEqual(2, result[1].Quantity);
            Assert.AreEqual(1, result[1].WithPieces.Count);
            Assert.AreEqual("Arms_AD1", result[1].WithPieces[0].Piece);
        }
    }
}