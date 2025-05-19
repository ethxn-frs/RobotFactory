namespace RobotFactory.Utils {

    public static class Parser {

        public static Dictionary<string, int> ParseArguments(string arguments)
        {
            var result = new Dictionary<string, int>();
            if (string.IsNullOrWhiteSpace(arguments))
                return result;

            var entries = arguments.Split(',');

            foreach (var entry in entries)
            {
                var parts = entry.Trim().Split(' ', 2);
                if(parts.Length != 2) {
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
    }
}

