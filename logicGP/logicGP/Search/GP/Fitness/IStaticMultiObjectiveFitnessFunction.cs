using System.Collections;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface
    IStaticMultiObjectiveFitnessFunction<TFitness> : IFitnessFunction<TFitness>
    where TFitness : IEnumerable
{
}