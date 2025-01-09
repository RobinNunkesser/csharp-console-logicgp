using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

public class LogicGpPareto : IStaticMultiObjectiveFitnessFunction
{
    public int NumberOfObjectives { get; set; }

    public double[] Evaluate(IIndividual individual, IDataView data,
        string labelColumnName = DefaultColumnNames.Label)
    {
        NumberOfObjectives = individual.Genotype.Predictions[0].Length + 1;
        var predictions =
            ((LogicGpGenotype)individual.Genotype).PredictedClasses;
        var labels = data.GetColumn<float>(labelColumnName).ToList();
        var objectives = new double[NumberOfObjectives];
        for (var i = 0; i < predictions.Length; i++)
        {
            if (!(Math.Abs(predictions[i] - labels[i]) < 0.01)) continue;
            var index = DataFactory.Instance.Labels.IndexOf(labels[i]);
            objectives[index]++;
        }

        objectives[^1] = -individual.Size;

        individual.LatestKnownFitness = objectives;
        return objectives;
    }
}