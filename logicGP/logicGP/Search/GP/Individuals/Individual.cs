using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public class Individual : IIndividual
{
    public Individual(IGenotype genotype, IIndividual[]? parents)
    {
        Genotype = genotype;
    }

    public IGenotype Genotype { get; }
    public double[]? LatestKnownFitness { get; set; }

    public object Clone()
    {
        return new Individual((IGenotype)Genotype.Clone(), null);
    }

    public override string ToString()
    {
        return Genotype.ToString() ?? string.Empty;
    }
}