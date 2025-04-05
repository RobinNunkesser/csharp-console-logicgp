using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

public class RealTests
{
    protected double TestFlRw(IDataView data, string label,
        int generations = 100)
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMacroMulticlassTrainer>();
        trainer.Label = label;
        // This is only for testing purposes, in production this should be set to a higher value (e.g. 10000)
        trainer.MaxGenerations = generations;

        var columnData = DataViewExtensions
            .GetColumnAsString(data, trainer.Label).ToList();
        var uniqueValues =
            new HashSet<string>(
                columnData);
        var labels = uniqueValues.OrderBy(c => c).ToList();
        var bestAccuracy = 0.0;
        for (var j = 0; j < 10; j++)
        {
            var mlModel = trainer.Fit(data);
            Assert.IsNotNull(mlModel);
            var testResults = mlModel.Transform(data);
            var trueValues = testResults.GetColumn<string>("y").ToArray();
            var predictedValues =
                testResults.GetColumn<string>("PredictedLabel").ToList();

            var accuracies = new float[labels.Count];
            var counts = new int[labels.Count];

            for (var i = 0; i < predictedValues.Count; i++)
            {
                counts[labels.IndexOf(trueValues[i])]++;
                if (predictedValues[i] == trueValues[i])
                    accuracies[labels.IndexOf(trueValues[i])]++;
            }

            for (var i = 0; i < labels.Count; i++)
            {
                accuracies[i] /= counts[i];
                Console.WriteLine($"{labels[i]}: {accuracies[i]}");
            }

            var macroAccuracy = accuracies.Sum() / labels.Count;
            if (macroAccuracy > bestAccuracy) bestAccuracy = macroAccuracy;
            Console.WriteLine($"MacroAccuracy: {macroAccuracy}");
        }

        Console.WriteLine($"Best MacroAccuracy: {bestAccuracy}");
        return bestAccuracy;
    }
}