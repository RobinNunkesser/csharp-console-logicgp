using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Microsoft.ML;

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
            foreach (var individual in individuals)
            {
                // TODO: First fitness than transformer from individual


                // var predictions = model.Transform(fold.TestSet);
                //var metrics = mlContext.BinaryClassification.Evaluate(predictions);
                // if (metrics.Accuracy > bestAccuracy)
                // {
                //     bestAccuracy = metrics.Accuracy;
                //     bestModel = model;
                // }
            }
        }

        return new LogicGpTransformer(chosenIndividual);
    }
}