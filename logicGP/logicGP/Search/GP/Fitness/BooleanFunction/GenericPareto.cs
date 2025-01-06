using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness.BooleanFunction;

public class GenericPareto : IStaticMultiObjectiveFitnessFunction
{
    public int NumberOfObjectives { get; }

    public double[] Evaluate(IGenotype genotype, IDataView data,
        string labelColumnName = DefaultColumnNames.Label)
    {
        throw new NotImplementedException();
    }
}