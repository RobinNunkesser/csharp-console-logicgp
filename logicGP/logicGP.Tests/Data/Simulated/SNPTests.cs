using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

[TestClass]
public sealed class SNPTests
{
    private readonly IDataView _data;

    public SNPTests()
    {
        var mlContext = new MLContext();
        _data = mlContext.Data.LoadFromTextFile<SNPModelInput>(
            "/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/standard/SNPglm_2.csv",
            ',', true);
    }

    [TestInitialize]
    public void TestInitialize()
    {
    }


    [TestMethod]
    public void TestGPAS()
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider.GetRequiredService<LogicGpGpasBinaryTrainer>();
        trainer.Label = "y";
        var mlModel = trainer.Fit(_data);
        Assert.IsNotNull(mlModel);
        var testResults = mlModel.Transform(_data);
        var trueValues = testResults.GetColumn<uint>("y").ToArray();
        var predictedValues = testResults.GetColumn<float[]>("Score")
            .Select(score => score[0] >= 0.5 ? 1 : 0).ToArray();
        var mcr = 0F;

        for (var i = 0; i < predictedValues.Length; i++)
            if (predictedValues[i] != trueValues[i])
                mcr++;

        mcr /= predictedValues.Length;
        var acc = 1.0 - mcr;
        Console.WriteLine($"{acc}");
    }
}