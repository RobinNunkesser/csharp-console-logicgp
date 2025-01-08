using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

[TestClass]
public sealed class SNPSimulation
{
    [TestMethod]
    public void GPASSimulation()
    {
        var mlContext = new MLContext();
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider.GetRequiredService<LogicGpGpasBinaryTrainer>();

        for (var j = 1; j < 99; j++)
        {
            var trainData = mlContext.Data.LoadFromTextFile<ModelInput>(
                $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/SNPglm_{j}.csv",
                ',', true);
            var testData = mlContext.Data.LoadFromTextFile<ModelInput>(
                $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/SNPglm_{j + 1}.csv",
                ',', true);

            var mlModel = trainer.Fit(trainData);
            Assert.IsNotNull(mlModel);
            var testResults = mlModel.Transform(testData);
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
}