using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IIndividual : ICloneable
{
    public IGenotype Genotype { get; }
    public double[]? LatestKnownFitness { get; set; }
    int Size { get; }
    bool IsDominating(IIndividual otherIndividual);
}