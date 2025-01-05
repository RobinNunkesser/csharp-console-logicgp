using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpGenotype : IGenotype
{
    private readonly IPolynomial<float> _polynomial;

    public LogicGpGenotype(int classes)
    {
        var literal = LiteralRepository.Instance.GetRandomLiteral();
        var monomial = new LogicGpMonomial<float>(literal, classes);
        _polynomial = new LogicGpPolynomial<float>(monomial);
    }

    public override string ToString()
    {
        return _polynomial.ToString() ?? string.Empty;
    }
}