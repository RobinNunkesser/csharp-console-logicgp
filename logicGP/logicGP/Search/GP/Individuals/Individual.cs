using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

public class Individual : IIndividual
{
    public Individual(IGenotype genotype, IIndividual[]? parents)
    {
        Genotype = genotype;
    }

    public IGenotype Genotype { get; }

    public double[]? LatestKnownFitness
    {
        get => Genotype.LatestKnownFitness;
        set => Genotype.LatestKnownFitness = value;
    }

    public int Size => Genotype.Size;
    public int Generation { get; set; }

    public bool IsDominating(IIndividual otherIndividual)
    {
        var fitness = LatestKnownFitness;
        var otherFitness = otherIndividual.LatestKnownFitness;
        if (fitness == null || otherFitness == null)
            throw new InvalidOperationException("Fitness not set");
        //if (otherIndividual.Size > 10) return true;
        return !fitness.Where((t, i) => t < otherFitness[i]).Any();
    }

    public object Clone()
    {
        return new Individual((IGenotype)Genotype.Clone(), null);
    }

    public override string ToString()
    {
        return Genotype + $" Gen {Generation} " +
               $"Fitness {string.Join(",", LatestKnownFitness ?? Array.Empty<double>())}" ??
               string.Empty;
    }
}