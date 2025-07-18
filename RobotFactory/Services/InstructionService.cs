using RobotFactory.Models;
using RobotFactory.Services.Impl;

namespace RobotFactory.Services
{
    public class InstructionService : IInstructionService
    {
        public List<string> GenerateInstructions(string robotName, List<string> pieces)
        {
            var pieceMap = new Dictionary<string, Piece>();
            Piece? system = null;

            foreach (var name in pieces)
            {
                var p = new Piece(name);
                if (name.StartsWith("System_"))
                {
                    system = p;
                }
                else
                {
                    var key = name.Split('_')[0]; // "Core", "Generator", etc.
                    pieceMap[key] = p;
                }
            }

            // Construction de l'arbre d'assemblage
            IAssemblyComponent? current = pieceMap.GetValueOrDefault("Core");

            foreach (var type in new[] { "Generator", "Arms", "Legs" })
            {
                if (pieceMap.TryGetValue(type, out var p) && current != null)
                {
                    current = new Assembly(current, p);
                }
            }

            // Génération des instructions
            var instructions = new List<string> { $"PRODUCING {robotName}" };

            if (system is not null && pieceMap.TryGetValue("Core", out var core))
            {
                instructions.AddRange(system.ToInstructions());
                instructions.Add($"INSTALL {system.GetName()} {core.GetName()}");
            }

            if (current is not null)
                instructions.AddRange(current.ToInstructions());

            instructions.Add($"FINISHED {robotName}");

            Console.WriteLine("Structure d'assemblage :\n");
            current?.Print();
            Console.WriteLine();
            Assembly.ResetCounter();

            return instructions;
        }
    }
}