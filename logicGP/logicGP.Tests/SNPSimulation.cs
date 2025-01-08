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
        using var logWriter = new StreamWriter(
            "/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/logicgpgpasacc_log.txt");
        using var writer = new StreamWriter(
            "/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/logicgpgpasacc.txt");
        var mlContext = new MLContext();
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider.GetRequiredService<LogicGpGpasBinaryTrainer>();

        for (var j = 1; j < 99; j++)
        {
            var trainDataPath =
                $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/SNPglm_{j}.csv";
            logWriter.WriteLine($"Training on {trainDataPath}");
            var trainData = mlContext.Data.LoadFromTextFile<ModelInput>(
                trainDataPath,
                ',', true);
            var testDataPath =
                $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/SNPglm_{j + 1}.csv";
            logWriter.WriteLine($"Testing on {testDataPath}");
            var testData = mlContext.Data.LoadFromTextFile<ModelInput>(
                testDataPath,
                ',', true);

            var mlModel = trainer.Fit(trainData);
            Assert.IsNotNull(mlModel);
            logWriter.WriteLine(((LogicGpTransformer)mlModel).Model.ToString());
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
            writer.WriteLine($"{acc}");
            writer.Flush();
            logWriter.WriteLine($"Accuracy: {acc}");
            logWriter.Flush();
        }
    }
}