using System.Text.RegularExpressions;
using RobotFactory.Models;

namespace RobotFactory.Utils
{
    public static class Parser
    {
        public static Dictionary<string, int> ParseArguments(string arguments)
        {
            var result = new Dictionary<string, int>();
            if (string.IsNullOrWhiteSpace(arguments))
                return result;

            var entries = arguments.Split(',');

            foreach (var entry in entries)
            {
                var parts = entry.Trim().Split(' ', 2);
                if (parts.Length != 2)
                {
                    Console.WriteLine($"ERROR: invalid argument provided ({string.Join(" ", parts)})");
                    return new Dictionary<string, int>();
                }

                if (int.TryParse(parts[0], out int quantity))
                {
                    string robotName = parts[1].Trim().ToUpper();
                    if (result.ContainsKey(robotName))
                        result[robotName] += quantity;
                    else
                        result[robotName] = quantity;
                }
            }

            return result;
        }

        public static List<ParsedRobotOrder> ParseComplexArguments(string input)
        {
            var result = new List<ParsedRobotOrder>();

            if (string.IsNullOrWhiteSpace(input)) return result;

            var robotChunks = input.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawChunk in robotChunks)
            {
                var chunk = rawChunk.Trim();

                // S�paration principale : quantit� + nom du robot
                var matchMain = Regex.Match(chunk, @"^(\d+)\s+([A-Za-z0-9\-]+)");
                if (!matchMain.Success)
                {
                    Console.WriteLine($"ERROR: Invalid robot format in '{chunk}'");
                    continue;
                }

                var parsed = new ParsedRobotOrder
                {
                    Quantity = int.Parse(matchMain.Groups[1].Value),
                    RobotName = matchMain.Groups[2].Value.ToUpper()
                };

                // Gestion des WITH, WITHOUT, REPLACE
                ParseModifier(chunk, "WITH", parsed.WithPieces);
                ParseModifier(chunk, "WITHOUT", parsed.WithoutPieces);
                ParseReplaceModifier(chunk, parsed.ReplacePieces);

                result.Add(parsed);
            }

            return result;
        }

        private static void ParseModifier(string chunk, string modifier, List<(int, string)> list)
        {
            var match = Regex.Match(chunk, @$"{modifier}\s+([0-9A-Za-z ,_-]+)", RegexOptions.IgnoreCase);
            if (!match.Success) return;

            var args = match.Groups[1].Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var arg in args)
            {
                var parts = arg.Trim().Split(' ', 2);
                if (parts.Length == 2 && int.TryParse(parts[0], out int qty))
                {
                    list.Add((qty, parts[1].Trim()));
                }
            }
        }

        private static void ParseReplaceModifier(string chunk, List<(int, string, string)> list)
        {
            var match = Regex.Match(chunk, @"REPLACE\s+([0-9A-Za-z ,_-]+)", RegexOptions.IgnoreCase);
            if (!match.Success) return;

            var args = match.Groups[1].Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i + 1 < args.Length; i += 2)
            {
                var fromParts = args[i].Trim().Split(' ', 2);
                var toParts = args[i + 1].Trim().Split(' ', 2);

                if (fromParts.Length == 2 && toParts.Length == 2 &&
                    int.TryParse(fromParts[0], out int qty) &&
                    int.TryParse(toParts[0], out int qtyToReplace))
                {
                    // On prend le min entre les deux quantit�s, ou on pourrait aussi le r�p�ter plusieurs fois
                    list.Add((Math.Min(qty, qtyToReplace), fromParts[1].Trim(), toParts[1].Trim()));
                }
            }
        }
    }
}