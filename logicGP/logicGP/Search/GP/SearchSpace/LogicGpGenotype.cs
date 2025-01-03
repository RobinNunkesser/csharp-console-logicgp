using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpGenotype : IGenotype
{
    private IPolynomial<float> _polynomial;

    public LogicGpGenotype()
    {
        var literal = LiteralRepository.Instance.GetRandomLiteral();
        var monomial = new LogicGpMonomial<float>(literal);
        _polynomial = new LogicGpPolynomial<float>(monomial);
    }
}