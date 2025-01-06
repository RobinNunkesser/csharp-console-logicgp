using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public class Accuracy : IStaticSingleObjectiveFitnessFunction
{
    public int NumberOfObjectives { get; } = 1;

    double[] IFitnessFunction.Evaluate(IGenotype genotype, IDataView data,
        string labelColumnName)
    {
        throw new NotImplementedException();
    }
}