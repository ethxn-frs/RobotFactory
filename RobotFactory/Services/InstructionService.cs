using RobotFactory.Models;
using RobotFactory.Services.Impl;

namespace RobotFactory.Services
{
    public class InstructionService : IInstructionService
    {
        public List<string> GenerateInstructions(string robotName, List<string> pieces)
        {
            Piece? core = null, generator = null, arms = null, legs = null, system = null;

            var components = new List<IAssemblyComponent>();
            foreach (var piece in pieces)
            {
                var p = new Piece(piece);
                if (piece.StartsWith("Core_")) core = p;
                else if (piece.StartsWith("Generator_")) generator = p;
                else if (piece.StartsWith("Arms_")) arms = p;
                else if (piece.StartsWith("Legs_")) legs = p;
                else if (piece.StartsWith("System_")) system = p;
                else components.Add(p);
            }

            if (core != null) components.Add(core);
            if (generator != null) components.Add(generator);
            if (arms != null) components.Add(arms);
            if (legs != null) components.Add(legs);

            IAssemblyComponent? current = core;
            if (generator != null && current != null)
                current = new Assembly(current, generator);
            if (arms != null && current != null)
                current = new Assembly(current, arms);
            if (legs != null && current != null)
                current = new Assembly(current, legs);

            var instructions = new List<string>
            {
                $"PRODUCING {robotName}"
            };

            if (system != null && core != null)
            {
                instructions.AddRange(system.ToInstructions());
                instructions.Add($"INSTALL {system.GetName()} {core.GetName()}");
            }

            if (current != null)
                instructions.AddRange(current.ToInstructions());

            instructions.Add($"FINISHED {robotName}");
            Console.WriteLine("Structure d'assemblage :");
            Console.WriteLine("");
            current?.Print();
            Console.WriteLine("");
            Assembly.ResetCounter();
            return instructions;
        }
    }
}