namespace RobotFactory.Models.Decorators;

public class WithoutPiecesDecorator : IRobot
{
    private readonly IRobot _inner;
    private readonly List<string> _piecesToRemove;

    public WithoutPiecesDecorator(IRobot inner, List<string> piecesToRemove)
    {
        _inner = inner;
        _piecesToRemove = piecesToRemove;
    }

    public string Name => _inner.Name;
    public string Category => _inner.Category;

    public List<string> GetPieces()
    {
        return _inner.GetPieces().Where(p => !_piecesToRemove.Contains(p)).ToList();
    }
}