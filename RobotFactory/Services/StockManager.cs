using System.Runtime.CompilerServices;
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

        private void InitializeStock()
        {
            _pieceStock = new Dictionary<string, int>
            {

                { "Core_CM1", 10 }, { "Core_CD1", 10 }, { "Core_CI1", 10 },
                { "Generator_GM1", 10 }, { "Generator_GD1", 10 }, { "Generator_GI1", 10 },
                { "Arms_AM1", 10 }, { "Arms_AD1", 10 }, { "Arms_AI1", 10 },
                { "Legs_LM1", 10 }, { "Legs_LD1", 10 }, { "Legs_LI1", 10 },
                { "System_SB1", 20 },
                { "Core_CM1", 10 },
                { "Core_CD1", 10 },
                { "Core_CI1", 10 },
                { "Generator_GM1", 10 },
                { "Generator_GD1", 10 },
                { "Generator_GI1", 10 },
                { "Arms_AM1", 10 },
                { "Arms_AD1", 10 },
                { "Arms_AI1", 10 },
                { "Legs_LM1", 10 },
                { "Legs_LD1", 10 },
                { "Legs_LI1", 10 },
                { "System_SB1", 20 },
                { "System_SM1", 20 },
                { "System_SD1", 20 },
                { "System_SI1", 20 }
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

            if(requestedRobots.Count < 1) {
                return;
            }

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

                if (!piecesPerRobots.ContainsKey(robotNameUpper)) {
                    piecesPerRobots[robotNameUpper] = new Dictionary<string, int>();
                }

                var robot = _robotTemplates[robotNameUpper];
                foreach (var piece in robot.RequiredPieces)
                {
                    // 1: Adding pieces to robot
                    if (!piecesPerRobots[robotNameUpper].ContainsKey(piece)) {
                        piecesPerRobots[robotNameUpper][piece] = quantity;
                    } else {
                        piecesPerRobots[robotNameUpper][piece] += quantity;
                    }
                    // 2: Adding pieces to total pieces
                    if(!totalPiecesNeeded.ContainsKey(piece)) {
                        totalPiecesNeeded[piece] = quantity;
                    } else {
                        totalPiecesNeeded[piece] += quantity;
                    }
                    
                }
            }

            Console.WriteLine("Pièces nécessaires :");
            foreach (var (pieceName, quantity) in totalPiecesNeeded)
                Console.WriteLine($"{quantity} {pieceName}");
                
            Console.WriteLine("\nPièces nécessaires :");

            foreach (var (robotName, quantityRobot) in requestedRobots) {
                Console.WriteLine($"{quantityRobot} {robotName} :");
                foreach(var (pieceName, quantityPiece) in piecesPerRobots[robotName]) {
                    Console.WriteLine($"{quantityPiece} {pieceName}");
                }
            }
            Console.WriteLine("Total :");
            foreach(var (pieceName, quantityPiece) in totalPiecesNeeded) {
                Console.WriteLine($"{quantityPiece} {pieceName}");
            }
        }

        public void DisplayInstructions(Dictionary<string, int> requestedRobots)
        {

            foreach (var (robotName, quantity) in requestedRobots)
            {
                if (!_robotTemplates.ContainsKey(robotName))
                {
                    Console.WriteLine($"ERROR `{robotName}` is not a recognized robot");
                    return;
                }

                for (int i = 0; i < quantity; i++)
                {
                    var robot = _robotTemplates[robotName];
                    var builder = new RobotAssemblyBuilder();
                    builder.Start(robot.Name);

                    string? core = null;
                    string? generator = null;
                    string? arms = null;
                    string? legs = null;
                    string? system = null;

                    foreach (var piece in robot.RequiredPieces)
                    {
                        builder.AddGetOutStock(piece);
                        if (piece.StartsWith("Core_")) core = piece;
                        else if (piece.StartsWith("Generator_")) generator = piece;
                        else if (piece.StartsWith("Arms_")) arms = piece;
                        else if (piece.StartsWith("Legs_")) legs = piece;
                        else if (piece.StartsWith("System_")) system = piece;
                    }

                    if (core != null && system != null)
                        builder.AddInstall(system, core);

                    if (core != null && generator != null)
                        builder.AddAssemble(core, generator);

                    if (arms != null)
                        builder.AddAssemble(arms);

                    if (legs != null)
                        builder.AddAssemble(legs);

                    builder.Finish(robot.Name);
                    foreach (var instruction in builder.Build())
                        Console.WriteLine(instruction);
                        if (piece.StartsWith("System_"))
                        {
                            Console.WriteLine(
                                $"INSTALL {piece} {robot.RequiredPieces[0]}");
                        }
                        else
                        {
                            Console.WriteLine($"GET_OUT_STOCK 1 {piece}");
                        }
                    }

                    var currentPiece = robot.RequiredPieces[0];
                    var pieceToAssemble = robot.RequiredPieces[1];
                    var assemblyName = "TMP1";

                    for(int indexPiece=2; indexPiece<robot.RequiredPieces.Count; indexPiece++) {

                        Console.WriteLine($"ASSEMBLE {assemblyName} {currentPiece} {pieceToAssemble}");
                        assemblyName = $"TMP{indexPiece}";
                        currentPiece = $"TMP{indexPiece-1}";
                        pieceToAssemble = robot.RequiredPieces[indexPiece];
                    }

                    Console.WriteLine($"FINISHED {robot.Name}");
                }
            }
        }

        public void VerifyOrder(Dictionary<string, int> requestedRobots)
        {
            var totalPiecesNeeded = new Dictionary<string, int>();

            foreach (var (robotName, quantity) in requestedRobots)
            {
                if (!_robotTemplates.ContainsKey(robotName))
                {
                    Console.WriteLine($"ERROR `{robotName}` is not a recognized robot");
                    return;
                }

                var robot = _robotTemplates[robotName];
                foreach (var piece in robot.RequiredPieces)
                {
                    if (!totalPiecesNeeded.ContainsKey(piece))
                        totalPiecesNeeded[piece] = 0;

                    totalPiecesNeeded[piece] += quantity;
                }
            }

            foreach (var (pieceName, quantityNeeded) in totalPiecesNeeded)
            {
                if (!_pieceStock.ContainsKey(pieceName) || _pieceStock[pieceName] < quantityNeeded)
                {
                    Console.WriteLine("UNAVAILABLE");
                    return;
                }
            }

            Console.WriteLine("AVAILABLE");
        }

        public void Produce(Dictionary<string, int> requestedRobots)
        {
            var totalPiecesNeeded = new Dictionary<string, int>();

            foreach (var (robotName, quantity) in requestedRobots)
            {
                if (!_robotTemplates.ContainsKey(robotName))
                {
                    Console.WriteLine($"ERROR `{robotName}` is not a recognized robot");
                    return;
                }

                var robot = _robotTemplates[robotName];
                foreach (var piece in robot.RequiredPieces)
                {
                    if (!totalPiecesNeeded.ContainsKey(piece))
                        totalPiecesNeeded[piece] = 0;

                    totalPiecesNeeded[piece] += quantity;
                }
            }

            foreach (var (pieceName, quantityNeeded) in totalPiecesNeeded)
            {
                if (!_pieceStock.ContainsKey(pieceName) || _pieceStock[pieceName] < quantityNeeded)
                {
                    Console.WriteLine("ERROR Not enough stock to produce the requested robots.");
                    return;
                }
            }

            foreach (var (pieceName, quantityNeeded) in totalPiecesNeeded)
                _pieceStock[pieceName] -= quantityNeeded;

            foreach (var (robotName, quantity) in requestedRobots)
                _robotStock[robotName] += quantity;

            Console.WriteLine("STOCK_UPDATED");
        }
    }
}