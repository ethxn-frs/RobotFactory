namespace RobotFactory.Models;

public class Assembly : IAssemblyComponent
{
    private static int _tmpCounter = 1;

    private string _tmpName;
    private IAssemblyComponent _left;
    private IAssemblyComponent _right;

    public static void ResetCounter() => _tmpCounter = 1;

    public Assembly(IAssemblyComponent left, IAssemblyComponent right)
    {
        _left = left;
        _right = right;
        _tmpName = $"TMP{_tmpCounter++}";
    }

    public string GetName() => _tmpName;

    public List<string> ToInstructions()
    {
        var result = new List<string>();
        result.AddRange(_left.ToInstructions());
        result.AddRange(_right.ToInstructions());
        result.Add($"ASSEMBLE {_tmpName} {_left.GetName()} {_right.GetName()}");
        return result;
    }

    public void Print(string indent = "", bool isLast = true)
    {
        Console.WriteLine($"{indent}└── {_tmpName}");
        indent += isLast ? "    " : "│   ";
        _left.Print(indent, false);
        _right.Print(indent, true);
    }
}