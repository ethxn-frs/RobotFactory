namespace RobotFactory.Models.Decorators;

public class WithPiecesDecorator : IRobot
{
    private readonly IRobot _inner;
    private readonly List<string> _piecesToAdd;

    public WithPiecesDecorator(IRobot inner, List<string> piecesToAdd)
    {
        _inner = inner;
        _piecesToAdd = piecesToAdd;
    }

    public string Name => _inner.Name;
    public string Category => _inner.Category;

    public List<string> GetPieces()
    {
        var basePieces = _inner.GetPieces();
        basePieces.AddRange(_piecesToAdd);
        return basePieces;
    }
}