using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpFlrwBinaryTrainer(
    LogicGpAlgorithm algorithm)
    : LogicGpTrainerBase<ITransformer>
{
    public override ITransformer Fit(IDataView input)
    {
        var k = 5; // Number of folds
        var mlContext = new MLContext();
        var cvResults = mlContext.Data.CrossValidationSplit(input, k);
        IIndividual chosenIndividual = null;
        double bestAccuracy = 0;

        foreach (var fold in cvResults)
        {
            var individuals = algorithm.Fit(fold.TrainSet);
            foreach (var literal in DataFactory.Instance.Literals)
                literal.GeneratePredictions(
                    fold.TestSet.GetColumn<float>(literal.Label).ToList());
            var fitness = new LogicGpPareto();
            foreach (var individual in individuals)
            {
                ((LogicGpGenotype)individual.Genotype)
                    .UpdatePredictionsRecursively();
                individual.Generation = 0;
                var fitnessValue = ((IFitnessFunction)fitness).Evaluate(
                    individual,
                    fold.TestSet, "y");
                var accuracy = 0.0;
                for (var i = 0; i < fitnessValue.Length - 1; i++)
                    accuracy += fitnessValue[i];
                individual.LatestKnownFitness =
                    new[] { accuracy, fitnessValue[^1] };
            }

            var selection = new ParetoFrontSelection();
            var selectedIndividuals = selection.Process(individuals);
        }

        // TODO: First fitness than transformer from individual

/*                var pipeline =
                    mlContext.Transforms.CustomMapping(mapping, "mapping");
                var transformedData = pipeline.Fit(fold.TestSet)
                    .Transform(fold.TestSet);*/
        /*var model = new LogicGpTransformer(individual);
        var predictions = model.Transform(fold.TestSet);
        var metrics =
            mlContext.MulticlassClassification.Evaluate(predictions,
                "y");
        var accuracy = metrics.MacroAccuracy;*/
        //var metrics = mlContext.BinaryClassification.Evaluate(predictions);
        // if (metrics.Accuracy > bestAccuracy)
        // {
        //     bestAccuracy = metrics.Accuracy;
        //     bestModel = model;
        // }

        return new LogicGpTransformer(chosenIndividual);
    }
}