using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

[TestClass]
public class CarEvaluationTests
{
    private readonly IDataView _data;

    public CarEvaluationTests()
    {
        var mlContext = new MLContext();
        _data = mlContext.Data.LoadFromTextFile<CarEvaluationModelInput>(
            "/Users/nunkesser/repos/work/articles/logicgp/data/ucimlrepo/car_evaluation.csv",
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider.GetRequiredService<LogicGpFlrwMulticlassTrainer>();
        trainer.Label = "class";
        var mlModel = trainer.Fit(_data);
        Assert.IsNotNull(mlModel);
        var testResults = mlModel.Transform(_data);
        var trueValues = testResults.GetColumn<uint>("y").ToArray();
        var predictedValues = testResults.GetColumn<float[]>("Score")
            .Select(score => score[0] >= 0.5 ? 0 : 1).ToArray();
        var mcr = 0F;

        for (var i = 0; i < predictedValues.Length; i++)
            if (predictedValues[i] != trueValues[i])
                mcr++;

        mcr /= predictedValues.Length;
        var acc = 1.0 - mcr;
        Console.WriteLine($"{acc}");
    }
}