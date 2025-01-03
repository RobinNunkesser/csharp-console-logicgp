namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpPolynomial<TCategory> : IPolynomial<TCategory>
{
    public LogicGpPolynomial(LogicGpMonomial<TCategory> monomial)
    {
        Monomials = [monomial];
    }

    public List<IMonomial<TCategory>> Monomials { get; set; }
}