using RobotFactory.Models;

namespace RobotFactoryTests.Models.Composite
{
    [TestClass]
    public class AssemblyTests
    {
        [TestMethod]
        public void Piece_ShouldReturnCorrectNameAndInstruction()
        {
            var piece = new Piece("Core_CM1");

            Assert.AreEqual("Core_CM1", piece.GetName());
            var instructions = piece.ToInstructions();
            Assert.AreEqual(1, instructions.Count);
            Assert.AreEqual("GET_OUT_STOCK 1 Core_CM1", instructions[0]);
        }

        [TestMethod]
        public void Assembly_ShouldGenerateAssembleInstruction()
        {
            var core = new Piece("Core_CM1");
            var generator = new Piece("Generator_GM1");
            var assembly = new Assembly(core, generator);

            var instructions = assembly.ToInstructions();

            Assert.AreEqual(3, instructions.Count);
            Assert.AreEqual("GET_OUT_STOCK 1 Core_CM1", instructions[0]);
            Assert.AreEqual("GET_OUT_STOCK 1 Generator_GM1", instructions[1]);
            Assert.IsTrue(instructions[2].StartsWith("ASSEMBLE TMP"));
            Assert.IsTrue(instructions[2].Contains("Core_CM1"));
            Assert.IsTrue(instructions[2].Contains("Generator_GM1"));
        }

        [TestMethod]
        public void Assembly_Print_ShouldOutputTreeStructure()
        {
            var core = new Piece("Core_CM1");
            var generator = new Piece("Generator_GM1");
            var arms = new Piece("Arms_AM1");

            var level1 = new Assembly(core, generator);
            var level2 = new Assembly(level1, arms);

            using var sw = new StringWriter();
            Console.SetOut(sw);

            level2.Print();

            var output = sw.ToString();
            Assert.IsTrue(output.Contains("TMP")); // TMPX apparaîtra
            Assert.IsTrue(output.Contains("Core_CM1"));
            Assert.IsTrue(output.Contains("Generator_GM1"));
            Assert.IsTrue(output.Contains("Arms_AM1"));
        }

        [TestMethod]
        public void ResetCounter_ShouldRestartTMPNames()
        {
            var a1 = new Assembly(new Piece("A"), new Piece("B"));
            var firstTMP = a1.GetName();

            Assembly.ResetCounter(); // reset le compteur TMP

            var a2 = new Assembly(new Piece("C"), new Piece("D"));
            var secondTMP = a2.GetName();

            Assert.AreEqual("TMP1", secondTMP);
        }
    }
}