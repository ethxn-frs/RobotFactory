namespace RobotFactory.Models;

public interface IAssemblyComponent
{
    string GetName();
    List<string> ToInstructions();
    void Print(string indent = "", bool isLast = true);
}