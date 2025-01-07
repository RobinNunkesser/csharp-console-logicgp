namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public interface IGenotype : ICloneable
{
    public float[][] Predictions { get; }

    public void UpdatePredictions();
}