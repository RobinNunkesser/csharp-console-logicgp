using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public class Accuracy : IStaticSingleObjectiveFitnessFunction
{
    public int NumberOfObjectives { get; } = 1;
    public string LabelColumnName { get; set; } = DefaultColumnNames.Label;

    double[] IFitnessFunction.Evaluate(IIndividual individual, IDataView data)
    {
        throw new NotImplementedException();
    }
}