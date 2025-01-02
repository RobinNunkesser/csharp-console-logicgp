using Italbytz.Adapters.Algorithms.AI.Search.GP.Population;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IStaticMultiObjectiveFitnessFunction : IFitnessFunction
{
    public double[] Evaluate(IGenotype genotype);
}