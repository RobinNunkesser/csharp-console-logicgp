using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public interface IFitnessFunction
{
    public int NumberOfObjectives { get; }

    public string LabelColumnName { get; set; }

    public double[] Evaluate(IIndividual individual, IDataView data);
}