using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public class Individual : IIndividual
{
    public Individual(IGenotype genotype, IIndividual[]? parents)
    {
        Genotype = genotype;
    }

    public IGenotype Genotype { get; }

    public override string ToString()
    {
        return Genotype.ToString() ?? string.Empty;
    }
}