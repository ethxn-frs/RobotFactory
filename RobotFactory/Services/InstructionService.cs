using RobotFactory.Builder;
using RobotFactory.Services.Impl;

namespace RobotFactory.Services
{
    public class InstructionService : IInstructionService
    {
        public List<string> GenerateInstructions(string robotName, List<string> pieces)
        {
            var builder = new RobotAssemblyBuilder();
            builder.Start(robotName);

            string? core = null, generator = null, arms = null, legs = null, system = null;

            foreach (var piece in pieces)
            {
                builder.AddGetOutStock(piece);
                if (piece.StartsWith("Core_")) core = piece;
                else if (piece.StartsWith("Generator_")) generator = piece;
                else if (piece.StartsWith("Arms_")) arms = piece;
                else if (piece.StartsWith("Legs_")) legs = piece;
                else if (piece.StartsWith("System_")) system = piece;
            }

            if (core != null && system != null) builder.AddInstall(system, core);
            if (core != null && generator != null) builder.AddAssemble(core, generator);
            if (arms != null) builder.AddAssemble(arms);
            if (legs != null) builder.AddAssemble(legs);

            builder.Finish(robotName);
            return builder.Build();
        }
    }
}