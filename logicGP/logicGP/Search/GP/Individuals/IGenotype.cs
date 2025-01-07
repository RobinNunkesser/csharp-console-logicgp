namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public interface IGenotype : ICloneable
{
    public float[][] Predictions { get; }
    double[]? LatestKnownFitness { get; set; }
    int Size { get; }

    public void UpdatePredictions();
}