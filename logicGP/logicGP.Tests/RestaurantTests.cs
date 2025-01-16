using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

[TestClass]
public class RestaurantTests
{
    private readonly IDataView _data;

    public RestaurantTests()
    {
        var mlContext = new MLContext();
        _data = mlContext.Data.LoadFromTextFile<RestaurantModelInput>(
            "/Users/nunkesser/repos/work/articles/logicgp/data/restaurant/restaurantcompletecoded.csv",
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMacroMulticlassTrainer>();
        trainer.Label = "will_wait";
        var mlModel = trainer.Fit(_data);
        Assert.IsNotNull(mlModel);
        var testResults = mlModel.Transform(_data);
        var trueValues = testResults.GetColumn<string>("y").ToArray();
        var predictedValues =
            testResults.GetColumn<string>("PredictedLabel").ToList();
        var mcr = 0F;

        for (var i = 0; i < predictedValues.Count; i++)
            if (predictedValues[i] != trueValues[i])
                mcr++;

        mcr /= predictedValues.Count;
        var acc = 1.0 - mcr;
        Console.WriteLine($"{acc}");
    }
}