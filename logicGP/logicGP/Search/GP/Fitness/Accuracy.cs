using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public class Accuracy : IStaticSingleObjectiveFitnessFunction<double>
{
    public double Evaluate(IGenotype genotype, IDataView data,
        string labelColumnName = DefaultColumnNames.Label,
        string scoreColumnName = DefaultColumnNames.Score,
        string probabilityColumnName = DefaultColumnNames.Probability,
        string predictedLabelColumnName = DefaultColumnNames.PredictedLabel)
    {
        throw new NotImplementedException();
    }
}