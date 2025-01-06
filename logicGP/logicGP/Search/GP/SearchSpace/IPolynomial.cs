namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public interface IPolynomial<TCategory>
{
    public List<IMonomial<TCategory>> Monomials { get; set; }
    public float[][] Predictions { get; set; }
    public IPolynomial<TCategory> Clone();
}