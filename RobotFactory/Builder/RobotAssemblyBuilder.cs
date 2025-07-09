namespace RobotFactory.Builder
{
    public class RobotAssemblyBuilder
    {
        private List<string> _instructions = new();
        private int _tmpCounter = 1;
        private string? _currentAssembly;

        public RobotAssemblyBuilder Start(string robotName)
        {
            _instructions.Add($"PRODUCING {robotName}");
            _currentAssembly = null;
            _tmpCounter = 1;
            return this;
        }

        public RobotAssemblyBuilder AddGetOutStock(string piece)
        {
            _instructions.Add($"GET_OUT_STOCK 1 {piece}");
            return this;
        }

        public RobotAssemblyBuilder AddInstall(string system, string target)
        {
            _instructions.Add($"INSTALL {system} {target}");
            return this;
        }

        public RobotAssemblyBuilder AddAssemble(string piece1, string piece2)
        {
            string tmpName = $"TMP{_tmpCounter++}";
            _instructions.Add($"ASSEMBLE {tmpName} {piece1} {piece2}");
            _currentAssembly = tmpName;
            return this;
        }

        public RobotAssemblyBuilder AddAssemble(string piece)
        {
            if (_currentAssembly == null)
            {
                _currentAssembly = piece;
                return this;
            }

            string tmpName = $"TMP{_tmpCounter++}";
            _instructions.Add($"ASSEMBLE {tmpName} {_currentAssembly} {piece}");
            _currentAssembly = tmpName;
            return this;
        }

        public RobotAssemblyBuilder Finish(string robotName)
        {
            _instructions.Add($"FINISHED {robotName}");
            return this;
        }

        public List<string> Build() => _instructions;
    }
}