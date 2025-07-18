namespace RobotFactory.Models;

public class Piece : IAssemblyComponent
{
    private string _name;

    public Piece(string name) => _name = name;

    public string GetName() => _name;

    public List<string> ToInstructions() => new() { $"GET_OUT_STOCK 1 {_name}" };

    public void Print(string indent = "", bool isLast = true)
    {
        Console.WriteLine($"{indent}└── {_name}");
    }
}