using RobotFactory.Services;

namespace RobotFactoryTests.Services
{
    [TestClass]
    public class InstructionServiceTests
    {
        private InstructionService _instructionService;

        [TestInitialize]
        public void Setup()
        {
            _instructionService = new InstructionService();
        }

        [TestMethod]
        public void GenerateInstructions_ShouldIncludeBasicSteps()
        {
            var pieces = new List<string>
            {
                "Core_CM1", "Generator_GM1", "Arms_AM1", "Legs_LM1", "System_SB1"
            };

            using var sw = new StringWriter();
            Console.SetOut(sw);

            var result = _instructionService.GenerateInstructions("XM-1", pieces);
            var output = sw.ToString();

            Assert.IsTrue(result.Contains("PRODUCING XM-1"));
            Assert.IsTrue(result.Exists(x => x.StartsWith("GET_OUT_STOCK")));
            Assert.IsTrue(result.Exists(x => x.StartsWith("ASSEMBLE")));
            Assert.IsTrue(result.Exists(x => x.StartsWith("INSTALL")));
            Assert.IsTrue(result.Contains("FINISHED XM-1"));
            Assert.IsTrue(output.Contains("Structure d'assemblage"));
        }

        [TestMethod]
        public void GenerateInstructions_ShouldOmitInstall_WhenSystemMissing()
        {
            var pieces = new List<string>
            {
                "Core_CM1", "Generator_GM1", "Arms_AM1", "Legs_LM1"
            };

            var result = _instructionService.GenerateInstructions("XM-1", pieces);

            Assert.IsFalse(result.Exists(x => x.StartsWith("INSTALL")));
            Assert.IsTrue(result.Contains("PRODUCING XM-1"));
            Assert.IsTrue(result.Contains("FINISHED XM-1"));
        }

        [TestMethod]
        public void GenerateInstructions_ShouldOmitAssemble_WhenCoreMissing()
        {
            var pieces = new List<string>
            {
                "Generator_GM1", "System_SB1"
            };

            var result = _instructionService.GenerateInstructions("XM-1", pieces);

            Assert.IsFalse(result.Exists(x => x.StartsWith("ASSEMBLE")));
            Assert.IsFalse(result.Exists(x => x.StartsWith("INSTALL")));
            Assert.IsTrue(result.Contains("PRODUCING XM-1"));
            Assert.IsTrue(result.Contains("FINISHED XM-1"));
        }

        [TestMethod]
        public void GenerateInstructions_ShouldResetTMP_CounterEachTime()
        {
            var pieces1 = new List<string> { "Core_CM1", "Generator_GM1" };
            var pieces2 = new List<string> { "Core_CM1", "Generator_GM1" };

            var result1 = _instructionService.GenerateInstructions("R1", pieces1);
            var result2 = _instructionService.GenerateInstructions("R2", pieces2);

            // On s’assure que les TMP recommencent à TMP1 dans le second appel
            Assert.IsTrue(result1.Exists(x => x.Contains("TMP1")));
            Assert.IsTrue(result2.Exists(x => x.Contains("TMP1")));
        }
    }
}