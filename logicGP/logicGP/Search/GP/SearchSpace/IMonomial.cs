namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public interface IMonomial<TCategory>
{
    List<ILiteral<TCategory>> Literals { get; set; }
    float[] Weights { get; set; }
    public float[][] Predictions { get; set; }
}