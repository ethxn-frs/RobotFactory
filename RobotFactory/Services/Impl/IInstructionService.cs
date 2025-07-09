namespace RobotFactory.Services.Impl
{
    public interface IInstructionService
    {
        List<string> GenerateInstructions(string robotName, List<string> pieces);
    }
}