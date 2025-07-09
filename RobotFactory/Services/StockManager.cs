using RobotFactory.Models;
using RobotFactory.Services.Impl;

namespace RobotFactory.Services
{
    public class StockManager : IStockManager
    {
        private static StockManager? _instance;
        private readonly IRobotService _robotService;

        private Dictionary<string, int> _pieceStock = new();
        private Dictionary<string, int> _robotStock = new();

        public StockManager(IRobotService robotService)
        {
            _robotService = robotService;
            InitializeStock();
        }
        
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
                Robot robot;
                try
                {
                    robot = _robotService.GetBaseRobot(robotName.ToUpper());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR {ex.Message}");
                    return;
                }

                if (!piecesPerRobots.ContainsKey(robotName))
                    piecesPerRobots[robotName] = new Dictionary<string, int>();

                foreach (var piece in robot.RequiredPieces)
                {
                    if (!piecesPerRobots[robotName].ContainsKey(piece))
                        piecesPerRobots[robotName][piece] = quantity;
                    else
                        piecesPerRobots[robotName][piece] += quantity;

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
                Console.WriteLine($"{quantityRobot} {robotName} :");
                foreach (var (pieceName, quantityPiece) in piecesPerRobots[robotName])
                    Console.WriteLine($"{quantityPiece} {pieceName}");
            }

            Console.WriteLine("Total :");
            foreach (var (pieceName, quantityPiece) in totalPiecesNeeded)
                Console.WriteLine($"{quantityPiece} {pieceName}");
        }


        public bool IsAvailable(Dictionary<string, int> needed)
        {
            foreach (var (piece, qty) in needed)
                if (!_pieceStock.ContainsKey(piece) || _pieceStock[piece] < qty)
                    return false;
            return true;
        }

        public void Consume(Dictionary<string, int> needed)
        {
            foreach (var (piece, qty) in needed)
                _pieceStock[piece] -= qty;
        }

        public void AddRobots(string name, int qty)
        {
            if (_robotStock.ContainsKey(name)) _robotStock[name] += qty;
            else _robotStock[name] = qty;
        }
    }
}