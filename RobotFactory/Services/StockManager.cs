using RobotFactory.Models;
using RobotFactory.Builder;

namespace RobotFactory.Services
{
    public class StockManager : IStockManager
    {
        private static StockManager? _instance;
        private static readonly object _lock = new object();

        public static StockManager Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new StockManager();
                }
            }
        }

        private Dictionary<string, int> _pieceStock = new();
        private Dictionary<string, int> _robotStock = new();
        private Dictionary<string, Robot> _robotTemplates = new();

        public StockManager()
        {
            InitializeStock();
            InitializeRobots();
        }

        private Dictionary<string, string> _pieceCategories = new()
        {
            { "Core_CM1", "M" }, { "Core_CD1", "D" }, { "Core_CI1", "I" },
            { "Generator_GM1", "M" }, { "Generator_GD1", "D" }, { "Generator_GI1", "I" },
            { "Arms_AM1", "M" }, { "Arms_AD1", "D" }, { "Arms_AI1", "I" },
            { "Legs_LM1", "M" }, { "Legs_LD1", "D" }, { "Legs_LI1", "I" },
            { "System_SB1", "G" }, { "System_SM1", "M" }, { "System_SD1", "D" }, { "System_SI1", "I" }
        };
        private void InitializeStock()
        {
            _pieceStock = new Dictionary<string, int>
            {
                { "Core_CM1", 10 }, { "Core_CD1", 10 }, { "Core_CI1", 10 },
                { "Generator_GM1", 10 }, { "Generator_GD1", 10 }, { "Generator_GI1", 10 },
                { "Arms_AM1", 10 }, { "Arms_AD1", 10 }, { "Arms_AI1", 10 },
                { "Legs_LM1", 10 }, { "Legs_LD1", 10 }, { "Legs_LI1", 10 },
                { "System_SB1", 20 }, { "System_SM1", 20 }, { "System_SD1", 20 }, { "System_SI1", 20 }
            };

            _robotStock = new Dictionary<string, int>
            {
                { "XM-1", 0 }, { "RD-1", 0 }, { "WI-1", 0 }
            };
        }

        private void InitializeRobots()
        {
            var factory = new RobotFactory.Factory.Factory();
            _robotTemplates = new Dictionary<string, Robot>
            {
                { "XM-1", factory.CreateRobot("XM-1") },
                { "RD-1", factory.CreateRobot("RD-1") },
                { "WI-1", factory.CreateRobot("WI-1") }
            };
        }
        public static void ResetStocks()
        {
            if (_instance != null)
            {
                _instance.InitializeStock();
                _instance.InitializeRobots();
            }
        }

        private bool ValidatePieceCategories(string robotName, List<string> pieces, out string error)
        {
            error = "";

            if (!_robotTemplates.ContainsKey(robotName))
            {
                error = $"Robot `{robotName}` non reconnu";
                return false;
            }

            var robot = _robotTemplates[robotName];
            var category = robot.Category;

            foreach (var piece in pieces)
            {
                if (!_pieceCategories.ContainsKey(piece))
                {
                    error = $"Pièce `{piece}` inconnue";
                    return false;
                }

                var pieceCat = _pieceCategories[piece];

                if (piece.StartsWith("System_"))
                {
                    if (category == "D" && !(pieceCat is "D" or "G"))
                    {
                        error = $"Système `{piece}` incompatible avec robot domestique `{robotName}`";
                        return false;
                    }
                    if (category == "I" && !(pieceCat is "I" or "G"))
                    {
                        error = $"Système `{piece}` incompatible avec robot industriel `{robotName}`";
                        return false;
                    }
                    if (category == "M" && !(pieceCat is "M" or "G"))
                    {
                        error = $"Système `{piece}` incompatible avec robot militaire `{robotName}`";
                        return false;
                    }
                }
                else // Pièces classiques
                {
                    if (category == "D" && !(pieceCat is "D" or "I" or "G"))
                    {
                        error = $"Pièce `{piece}` incompatible avec robot domestique `{robotName}`";
                        return false;
                    }
                    if (category == "I" && !(pieceCat is "I" or "G"))
                    {
                        error = $"Pièce `{piece}` incompatible avec robot industriel `{robotName}`";
                        return false;
                    }
                    if (category == "M" && !(pieceCat is "M" or "I"))
                    {
                        error = $"Pièce `{piece}` incompatible avec robot militaire `{robotName}`";
                        return false;
                    }
                }
            }

            return true;
        }

        public void DisplayStocks()
        {
            Console.WriteLine("Robots disponibles :");
            foreach (var robot in _robotStock)
                Console.WriteLine($"{robot.Value} {robot.Key}");

            Console.WriteLine("\nPièces disponibles :");
            foreach (var piece in _pieceStock)
                Console.WriteLine($"{piece.Value} {piece.Key}");
        }

        public void DisplayNeededPieces(Dictionary<string, int> requestedRobots)
        {
            if (requestedRobots.Count < 1) return;

            var piecesPerRobots = new Dictionary<string, Dictionary<string, int>>();
            var totalPiecesNeeded = new Dictionary<string, int>();

            foreach (var (robotName, quantity) in requestedRobots)
            {
                var robotNameUpper = robotName.ToUpper();

                if (!_robotTemplates.ContainsKey(robotNameUpper))
                {
                    Console.WriteLine($"ERROR `{robotName}` is not a recognized robot");
                    return;
                }

                if (!piecesPerRobots.ContainsKey(robotNameUpper))
                    piecesPerRobots[robotNameUpper] = new Dictionary<string, int>();

                var robot = _robotTemplates[robotNameUpper];
                foreach (var piece in robot.RequiredPieces)
                {
                    if (!piecesPerRobots[robotNameUpper].ContainsKey(piece))
                        piecesPerRobots[robotNameUpper][piece] = quantity;
                    else
                        piecesPerRobots[robotNameUpper][piece] += quantity;

                    if (!totalPiecesNeeded.ContainsKey(piece))
                        totalPiecesNeeded[piece] = quantity;
                    else
                        totalPiecesNeeded[piece] += quantity;
                }
            }

            Console.WriteLine("Pièces nécessaires :");
            foreach (var (pieceName, quantity) in totalPiecesNeeded)
                Console.WriteLine($"{quantity} {pieceName}");

            Console.WriteLine("\nDétail par robot :");
            foreach (var (robotName, quantityRobot) in requestedRobots)
            {
                var upperName = robotName.ToUpper();
                Console.WriteLine($"{quantityRobot} {robotName} :");
                foreach (var (pieceName, quantityPiece) in piecesPerRobots[upperName])
                    Console.WriteLine($"{quantityPiece} {pieceName}");
            }

            Console.WriteLine("Total :");
            foreach (var (pieceName, quantityPiece) in totalPiecesNeeded)
                Console.WriteLine($"{quantityPiece} {pieceName}");
        }
        public void DisplayInstructions(List<ParsedRobotOrder> robotOrders)
        {
            foreach (var order in robotOrders)
            {
                if (!_robotTemplates.ContainsKey(order.RobotName))
                {
                    Console.WriteLine($"ERROR `{order.RobotName}` is not a recognized robot");
                    continue;
                }

                var pieces = GetModifiedRobotPieces(order);

                if (!ValidatePieceCategories(order.RobotName, pieces, out var error))
                {
                    Console.WriteLine($"ERROR {error}");
                    continue;
                }

                for (int i = 0; i < order.Quantity; i++)
                {
                    var builder = new RobotAssemblyBuilder();
                    builder.Start(order.RobotName);

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

                    builder.Finish(order.RobotName);
                    foreach (var instruction in builder.Build())
                        Console.WriteLine(instruction);
                }
            }
        }

        public void VerifyOrder(List<ParsedRobotOrder> robotOrders)
        {
            var totalNeeded = new Dictionary<string, int>();

            foreach (var order in robotOrders)
            {
                if (!_robotTemplates.ContainsKey(order.RobotName))
                {
                    Console.WriteLine($"ERROR `{order.RobotName}` is not a recognized robot");
                    return;
                }

                var pieces = GetModifiedRobotPieces(order);

                if (!ValidatePieceCategories(order.RobotName, pieces, out var error))
                {
                    Console.WriteLine($"ERROR {error}");
                    return;
                }

                foreach (var piece in pieces)
                {
                    if (!totalNeeded.ContainsKey(piece)) totalNeeded[piece] = 0;
                    totalNeeded[piece] += order.Quantity;
                }
            }

            foreach (var (piece, needed) in totalNeeded)
            {
                if (!_pieceStock.ContainsKey(piece) || _pieceStock[piece] < needed)
                {
                    Console.WriteLine("UNAVAILABLE");
                    return;
                }
            }

            Console.WriteLine("AVAILABLE");
        }
        public void Produce(List<ParsedRobotOrder> robotOrders)
        {
            var totalNeeded = new Dictionary<string, int>();

            foreach (var order in robotOrders)
            {
                if (!_robotTemplates.ContainsKey(order.RobotName))
                {
                    Console.WriteLine($"ERROR `{order.RobotName}` is not a recognized robot");
                    return;
                }

                var pieces = GetModifiedRobotPieces(order);

                if (!ValidatePieceCategories(order.RobotName, pieces, out var error))
                {
                    Console.WriteLine($"ERROR {error}");
                    return;
                }

                foreach (var piece in pieces)
                {
                    if (!totalNeeded.ContainsKey(piece)) totalNeeded[piece] = 0;
                    totalNeeded[piece] += order.Quantity;
                }
            }

            // Vérifie le stock
            foreach (var (piece, needed) in totalNeeded)
            {
                if (!_pieceStock.ContainsKey(piece) || _pieceStock[piece] < needed)
                {
                    Console.WriteLine("ERROR Not enough stock to produce the requested robots.");
                    return;
                }
            }

            // Mise à jour du stock
            foreach (var (piece, needed) in totalNeeded)
                _pieceStock[piece] -= needed;

            foreach (var order in robotOrders)
            {
                if (_robotStock.ContainsKey(order.RobotName))
                    _robotStock[order.RobotName] += order.Quantity;
                else
                    _robotStock[order.RobotName] = order.Quantity;
            }

            Console.WriteLine("STOCK_UPDATED");
        }


        private List<string> GetModifiedRobotPieces(ParsedRobotOrder order)
        {
            if (!_robotTemplates.TryGetValue(order.RobotName, out var baseRobot))
                throw new ArgumentException($"Robot inconnu : {order.RobotName}");

            // Copier les pièces de base
            var pieces = new List<string>(baseRobot.RequiredPieces);

            // WITHOUT : suppression
            foreach (var (qty, piece) in order.WithoutPieces)
            {
                for (int i = 0; i < qty; i++)
                    pieces.Remove(piece);
            }

            // REPLACE : suppression et ajout
            foreach (var (qty, fromPiece, toPiece) in order.ReplacePieces)
            {
                for (int i = 0; i < qty; i++)
                {
                    if (pieces.Contains(fromPiece))
                    {
                        pieces.Remove(fromPiece);
                        pieces.Add(toPiece);
                    }
                }
            }

            // WITH : ajout
            foreach (var (qty, piece) in order.WithPieces)
            {
                for (int i = 0; i < qty; i++)
                    pieces.Add(piece);
            }

            return pieces;
        }

    }
}
