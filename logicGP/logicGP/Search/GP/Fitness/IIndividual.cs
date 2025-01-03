using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IIndividual
{
    public IGenotype Genotype { get; }
}