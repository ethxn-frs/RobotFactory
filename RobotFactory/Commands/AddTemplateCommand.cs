using RobotFactory.Services.Impl;

namespace RobotFactory.Commands
{
    public class AddTemplateCommand : ICommand
    {
        private readonly IRobotService _robotService;
        public string Name => "ADD_TEMPLATE";

        public AddTemplateCommand(IRobotService robotService) => _robotService = robotService;

        public void Execute(string arguments)
        {
            var parts = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                Console.WriteLine("Format : ADD_TEMPLATE <Nom> <Pièce1> <Pièce2> ...");
                return;
            }

            var name = parts[0];
            var pieces = parts.Skip(1).ToList();
            _robotService.AddTemplate(name, pieces);
        }
    }
}