using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IStaticMultiObjectiveFitnessFunction : IFitnessFunction
{
    public double[] Evaluate(IGenotype genotype);
}