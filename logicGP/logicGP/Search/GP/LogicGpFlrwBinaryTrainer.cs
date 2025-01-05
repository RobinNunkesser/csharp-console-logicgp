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
        ITransformer bestModel = null;
        double bestAccuracy = 0;

        foreach (var fold in cvResults)
        {
            var individuals = algorithm.Fit(fold.TrainSet);
            foreach (var individual in individuals)
            {
                // TODO: Make transformer from individual
                // var predictions = model.Transform(fold.TestSet);
                // var metrics = mlContext.BinaryClassification.Evaluate(predictions);
                // if (metrics.Accuracy > bestAccuracy)
                // {
                //     bestAccuracy = metrics.Accuracy;
                //     bestModel = model;
                // }
            }
        }

        return bestModel;
        return new LogicGpTransformer();
    }
}