namespace RobotFactory.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute(string arguments);
    }
}