namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public interface IPolynomial<TCategory> : ICloneable
{
    public List<IMonomial<TCategory>> Monomials { get; set; }
    public float[][] Predictions { get; set; }
    IMonomial<TCategory> GetRandomMonomial();
    void UpdatePredictions();
}