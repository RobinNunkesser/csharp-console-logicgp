using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public class LogicGpPareto : IStaticMultiObjectiveFitnessFunction
{
    public IDataView LiteralData { get; set; }

    public int NumberOfObjectives { get; }

    public double[] Evaluate(IIndividual individual, IDataView data,
        string labelColumnName = DefaultColumnNames.Label)
    {
        return new[] { 0.0d };
    }
}