using System.Globalization;
using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using logicGP.Tests.Data.Simulated;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests.Unit.Data.Simulated;

[TestClass]
public sealed class SNPSimulationTests
{
    [TestMethod]
    public void TestSimulation1()
    {
        GPASSimulation("Simulation1", AppDomain.CurrentDomain.BaseDirectory);
    }

    [TestMethod]
    public void TestSimulation2()
    {
        GPASSimulation("Simulation2", AppDomain.CurrentDomain.BaseDirectory);
    }

    [TestMethod]
    public void TestSimulation3()
    {
        GPASSimulation("Simulation3", AppDomain.CurrentDomain.BaseDirectory);
    }


    public void GPASSimulation(string folder, string logFolder)
    {
        var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var path = Path.Combine(logFolder,
            $"logicgpgpasacc_{timeStamp}_log.txt");
        using var logWriter = new StreamWriter(path);
        path = Path.Combine(logFolder,
            $"logicgpgpasacc_{timeStamp}.csv");
        using var writer = new StreamWriter(path);
        writer.WriteLine("\"x\"");
        var mlContext = new MLContext();
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider.GetRequiredService<LogicGpGpasBinaryTrainer>();
        /*var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMicroMulticlassTrainer>();*/
        trainer.Label = "y";


        for (var j = 1; j < 101; j++)
        {
            var trainDataPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                $"Data/Simulated/{folder}", $"SNPglm_{j}.csv");
            logWriter.WriteLine($"Training on {trainDataPath}");
            var trainData = mlContext.Data.LoadFromTextFile<SNPModelInput>(
                trainDataPath,
                ',', true);
            var testIndex = j == 100 ? 1 : j + 1;
            var testDataPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $"Data/Simulated/{folder}", $"SNPglm_{testIndex}.csv");
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
            var acc = 0F;

            var columnData = testData.GetColumnAsString(trainer.Label).ToList();
            var uniqueValues =
                new HashSet<string>(
                    columnData);
            var labels = uniqueValues.OrderBy(c => c).ToList();
            var counts = new int[labels.Count];
            var accuracies = new float[labels.Count];

            for (var i = 0; i < predictedValues.Length; i++)
            {
                counts[labels.IndexOf(trueValues[i].ToString())]++;
                if (predictedValues[i] == trueValues[i])
                    accuracies[labels.IndexOf(trueValues[i].ToString())]++;
            }

            for (var i = 0; i < labels.Count; i++)
            {
                accuracies[i] /= counts[i];
                Console.WriteLine($"{labels[i]}: {accuracies[i]}");
            }

            var macroAccuracy = accuracies.Sum() / labels.Count;

            for (var i = 0; i < predictedValues.Length; i++)
                if (predictedValues[i] == trueValues[i])
                    acc++;

            acc /= predictedValues.Length;
            writer.WriteLine(
                macroAccuracy.ToString(CultureInfo.InvariantCulture));
            writer.Flush();
            logWriter.WriteLine($"Accuracy: {acc}");
            logWriter.Flush();
        }
    }
}