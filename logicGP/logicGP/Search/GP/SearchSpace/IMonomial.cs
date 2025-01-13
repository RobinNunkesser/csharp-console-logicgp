namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public interface IMonomial<TCategory> : ICloneable
{
    List<ILiteral<TCategory>> Literals { get; set; }
    float[] Weights { get; set; }
    public float[][] Predictions { get; set; }
    int Size { get; }
    void RandomizeWeights(bool restricted);
    void UpdatePredictions();
}