using System.Runtime.CompilerServices;
using RobotFactory.Models;

namespace RobotFactory.Services
{
    public class StockManager
    {
        private Dictionary<string, int> _pieceStock;
        private Dictionary<string, int> _robotStock;
        private Dictionary<string, Robot> _robotTemplates;

        public StockManager()
        {
            InitializeStock();
            InitializeRobots();
        }

        private void InitializeStock()
        {
            _pieceStock = new Dictionary<string, int>
            {
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
                { "XM-1", 0 },
                { "RD-1", 0 },
                { "WI-1", 0 }
            };
        }

        private void InitializeRobots()
        {
            _robotTemplates = new Dictionary<string, Robot>
            {
                {
                    "XM-1",
                    new Robot("XM-1",
                        new List<string> { "Core_CM1", "Generator_GM1", "Arms_AM1", "Legs_LM1", "System_SB1" })
                },
                {
                    "RD-1",
                    new Robot("RD-1",
                        new List<string> { "Core_CD1", "Generator_GD1", "Arms_AD1", "Legs_LD1", "System_SB1" })
                },
                {
                    "WI-1",
                    new Robot("WI-1",
                        new List<string> { "Core_CI1", "Generator_GI1", "Arms_AI1", "Legs_LI1", "System_SB1" })
                }
            };
        }

        public void DisplayStocks()
        {
            Console.WriteLine("Robots disponibles :");
            foreach (var robot in _robotStock)
            {
                Console.WriteLine($"{robot.Value} {robot.Key}");
            }

            Console.WriteLine("\nPièces disponibles :");
            foreach (var piece in _pieceStock)
            {
                Console.WriteLine($"{piece.Value} {piece.Key}");
            }
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
                    Console.WriteLine($"PRODUCING {robot.Name}");

                    foreach (var piece in robot.RequiredPieces)
                    {
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
            {
                _pieceStock[pieceName] -= quantityNeeded;
            }

            foreach (var (robotName, quantity) in requestedRobots)
            {
                _robotStock[robotName] += quantity;
            }

            Console.WriteLine("STOCK_UPDATED");
        }
    }
}