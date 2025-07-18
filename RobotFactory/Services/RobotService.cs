using RobotFactory.Models;
using RobotFactory.Models.Decorators;
using RobotFactory.Services.Impl;

namespace RobotFactory.Services
{
    public class RobotService : IRobotService
    {
        private readonly Dictionary<string, Robot> _robotTemplates;
        private readonly Dictionary<string, string> _pieceCategories;

        public RobotService()
        {
            var factory = new Factory.Factory();
            _robotTemplates = new Dictionary<string, Robot>
            {
                { "XM-1", factory.CreateRobot("XM-1") },
                { "RD-1", factory.CreateRobot("RD-1") },
                { "WI-1", factory.CreateRobot("WI-1") }
            };

            _pieceCategories = new()
            {
                { "Core_CM1", "M" }, { "Core_CD1", "D" }, { "Core_CI1", "I" },
                { "Generator_GM1", "M" }, { "Generator_GD1", "D" }, { "Generator_GI1", "I" },
                { "Arms_AM1", "M" }, { "Arms_AD1", "D" }, { "Arms_AI1", "I" },
                { "Legs_LM1", "M" }, { "Legs_LD1", "D" }, { "Legs_LI1", "I" },
                { "System_SB1", "G" }, { "System_SM1", "M" }, { "System_SD1", "D" }, { "System_SI1", "I" }
            };
        }

        public Robot GetBaseRobot(string robotName)
        {
            if (!_robotTemplates.TryGetValue(robotName, out var robot))
                throw new ArgumentException($"Robot inconnu : {robotName}");

            return robot;
        }

        public List<string> GetModifiedPieces(ParsedRobotOrder order)
        {
            var baseRobot = GetBaseRobot(order.RobotName);
            IRobot robot = new BasicRobotWrapper(baseRobot);

            if (order.WithoutPieces.Any())
                robot = new WithoutPiecesDecorator(robot, order.WithoutPieces.Select(x => x.Piece).ToList());

            if (order.ReplacePieces.Any())
                robot = new ReplacePiecesDecorator(robot,
                    order.ReplacePieces.Select(x => (x.FromPiece, x.ToPiece)).ToList());

            if (order.WithPieces.Any())
                robot = new WithPiecesDecorator(robot, order.WithPieces.Select(x => x.Piece).ToList());

            return robot.GetPieces();
        }

        public bool ValidateCategories(Robot robot, List<string> pieces, out string error)
        {
            error = "";
            foreach (var piece in pieces)
            {
                if (!_pieceCategories.ContainsKey(piece))
                {
                    error = $"Pièce inconnue : {piece}";
                    return false;
                }

                var category = _pieceCategories[piece];
                var rcat = robot.Category;

                if (piece.StartsWith("System_"))
                {
                    if (rcat == "D" && !(category is "D" or "G"))
                    {
                        error = $"Systeme {piece} incompatible (D)";
                        return false;
                    }

                    if (rcat == "I" && !(category is "I" or "G"))
                    {
                        error = $"Systeme {piece} incompatible (I)";
                        return false;
                    }

                    if (rcat == "M" && !(category is "M" or "G"))
                    {
                        error = $"Systeme {piece} incompatible (M)";
                        return false;
                    }
                }
                else
                {
                    if (rcat == "D" && !(category is "D" or "I" or "G"))
                    {
                        error = $"Pièce {piece} incompatible (D)";
                        return false;
                    }

                    if (rcat == "I" && !(category is "I" or "G"))
                    {
                        error = $"Pièce {piece} incompatible (I)";
                        return false;
                    }

                    if (rcat == "M" && !(category is "M" or "I"))
                    {
                        error = $"Pièce {piece} incompatible (M)";
                        return false;
                    }
                }
            }

            return true;
        }

        public void AddTemplate(string name, List<string> pieces)
        {
            if (_robotTemplates.ContainsKey(name))
            {
                Console.WriteLine($"ERROR Template déjà existant : {name}");
                return;
            }

            if (pieces.Count == 0)
            {
                Console.WriteLine("ERROR Aucun composant fourni.");
                return;
            }

            // Vérifie que toutes les pièces sont connues
            foreach (var piece in pieces)
            {
                if (!_pieceCategories.ContainsKey(piece))
                {
                    Console.WriteLine($"ERROR Pièce inconnue : {piece}");
                    return;
                }
            }

            // Trouve la catégorie à partir du Core
            var corePiece = pieces.FirstOrDefault(p => p.StartsWith("Core_"));
            if (corePiece == null)
            {
                Console.WriteLine("ERROR Aucun Core_ trouvé pour déterminer la catégorie.");
                return;
            }

            var category = _pieceCategories[corePiece];
            var robot = new Robot(name, pieces, category);
            _robotTemplates[name] = robot;

            Console.WriteLine($"TEMPLATE {name} ajouté avec succès.");
        }
    }
}