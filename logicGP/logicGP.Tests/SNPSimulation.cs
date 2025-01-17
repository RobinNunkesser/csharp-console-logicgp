using System.Globalization;
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
        //const string folder = "standard";
        //const string folder = "laumain_s1000_o15_p0225_n44";
        //const string folder = "lauinteraction_s1000_o15_p0225_n45_i14";
        //const string folder = "standard_mac";
        const string folder = "laumain_s1000_o15_p0225_n44_mac";
        //const string folder = "lauinteraction_s1000_o15_p0225_n45_i14_mac";
        var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        using var logWriter = new StreamWriter(
            $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/{folder}/logicgpgpasacc_{timeStamp}_log.txt");
        using var writer = new StreamWriter(
            $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/{folder}/logicgpgpasacc_{timeStamp}.csv");
        writer.WriteLine("\"x\"");
        var mlContext = new MLContext();
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        /*var trainer =
            serviceProvider.GetRequiredService<LogicGpGpasBinaryTrainer>();*/
        var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMacroMulticlassTrainer>();
        trainer.Label = "y";
        for (var j = 1; j < 100; j++)
        {
            var trainDataPath =
                $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/{folder}/SNPglm_{j}.csv";
            logWriter.WriteLine($"Training on {trainDataPath}");
            var trainData = mlContext.Data.LoadFromTextFile<SNPModelInput>(
                trainDataPath,
                ',', true);
            var testDataPath =
                $"/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/{folder}/SNPglm_{j + 1}.csv";
            logWriter.WriteLine($"Testing on {testDataPath}");
            var testData = mlContext.Data.LoadFromTextFile<SNPModelInput>(
                testDataPath,
                ',', true);

            var mlModel = trainer.Fit(trainData);
            Assert.IsNotNull(mlModel);
            logWriter.WriteLine(
                ((LogicGpGpasTransformer)mlModel).Model.ToString());
            var testResults = mlModel.Transform(testData);
            var trueValues = testResults.GetColumn<uint>("y").ToArray();
            var predictedValues = testResults.GetColumn<float[]>("Score")
                .Select(score => score[0] >= 0.5 ? 1 : 0).ToArray();
            var mcr = 0F;

            for (var i = 0; i < predictedValues.Length; i++)
                if (predictedValues[i] != trueValues[i])
                    mcr++;

            mcr /= predictedValues.Length;
            var acc = 1.0 - mcr;
            writer.WriteLine(acc.ToString(CultureInfo.InvariantCulture));
            writer.Flush();
            logWriter.WriteLine($"Accuracy: {acc}");
            logWriter.Flush();
        }
    }
}