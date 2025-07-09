namespace RobotFactory.Models.Decorators;

public class ReplacePiecesDecorator : IRobot
{
    private readonly IRobot _inner;
    private readonly List<(string from, string to)> _replacements;

    public ReplacePiecesDecorator(IRobot inner, List<(string from, string to)> replacements)
    {
        _inner = inner;
        _replacements = replacements;
    }

    public string Name => _inner.Name;
    public string Category => _inner.Category;

    public List<string> GetPieces()
    {
        var basePieces = _inner.GetPieces();

        foreach (var (from, to) in _replacements)
        {
            var index = basePieces.IndexOf(from);
            if (index >= 0)
            {
                basePieces[index] = to;
            }
        }

        return basePieces;
    }
}