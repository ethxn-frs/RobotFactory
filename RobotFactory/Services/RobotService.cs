using RobotFactory.Models;
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
            var pieces = new List<string>(baseRobot.RequiredPieces);

            foreach (var (qty, piece) in order.WithoutPieces)
                for (int i = 0; i < qty; i++)
                    pieces.Remove(piece);

            foreach (var (qty, from, to) in order.ReplacePieces)
                for (int i = 0; i < qty; i++)
                    if (pieces.Remove(from))
                        pieces.Add(to);

            foreach (var (qty, piece) in order.WithPieces)
                for (int i = 0; i < qty; i++)
                    pieces.Add(piece);

            return pieces;
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
    }
}