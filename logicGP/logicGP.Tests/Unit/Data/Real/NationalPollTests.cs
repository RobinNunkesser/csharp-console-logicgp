using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

[TestClass]
public class NationalPollTests
{
    private readonly IDataView _data;

    public NationalPollTests()
    {
        var mlContext = new MLContext();
        _data = mlContext.Data.LoadFromTextFile<NationalPollModelInput>(
            "/Users/nunkesser/repos/work/articles/logicgp/data/ucimlrepo/nationalpollnpha/national_poll_on_healthy_aging_npha.csv",
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
        trainer.Label = "Number_of_Doctors_Visited";

        var columnData = _data.GetColumnAsString(trainer.Label).ToList();
        var uniqueValues =
            new HashSet<string>(
                columnData);
        var labels = uniqueValues.OrderBy(c => c).ToList();
        for (var j = 0; j < 10; j++)
        {
            var mlModel = trainer.Fit(_data);
            Assert.IsNotNull(mlModel);
            var testResults = mlModel.Transform(_data);
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
            Console.WriteLine($"Macro Accuracy: {macroAccuracy}");
        }
    }

    [TestMethod]
    public void MakeTrainTestSets()
    {
        var mlContext = new MLContext();
        var cvResults = mlContext.Data.CrossValidationSplit(_data);
        var index = 0;
        foreach (var fold in cvResults)
        {
            index++;
            var trainData = fold.TrainSet;
            var testData = fold.TestSet;
            var trainDataPath =
                $"/Users/nunkesser/repos/work/articles/logicgp/data/ucimlrepo/carevaluation/car_evaluation_strings_train_{index}.csv";
            var testDataPath =
                $"/Users/nunkesser/repos/work/articles/logicgp/data/ucimlrepo/carevaluation/car_evaluation_strings_test_{index}.csv";
            using (var trainDataStream = new FileStream(trainDataPath,
                       FileMode.Create, FileAccess.Write))
            {
                mlContext.Data.SaveAsText(trainData, trainDataStream, ',');
            }

            using (var testDataStream = new FileStream(testDataPath,
                       FileMode.Create, FileAccess.Write))
            {
                mlContext.Data.SaveAsText(testData, testDataStream, ',');
            }
        }
    }

    [TestMethod]
    public void ParseMLRun()
    {
        var filePath =
            "/Users/nunkesser/repos/work/articles/logicgp/data/ucimlrepo/solarflare/mlnettraining_3.csv";
        using var reader = new StreamReader(filePath);
        var bestMacroaccuracy = new Dictionary<string, float>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var elements = line.Split(' ',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);
            var nextIsAccuracy = false;
            var currentAlgorithm = "";
            foreach (var element in elements)
            {
                if (element.Contains("|")) continue;
                var parsedValue = 0.0f;
                if (float.TryParse(element, out parsedValue))
                {
                    if (nextIsAccuracy)
                    {
                        nextIsAccuracy = false;
                        if (bestMacroaccuracy[currentAlgorithm] < parsedValue)
                            bestMacroaccuracy[currentAlgorithm] = parsedValue;
                    }

                    continue;
                }

                nextIsAccuracy = true;
                currentAlgorithm = element;
                if (!bestMacroaccuracy.ContainsKey(element))
                    bestMacroaccuracy[element] = 0.0f;
            }
        }

        foreach (var entry in bestMacroaccuracy)
            Console.WriteLine($"{entry.Key}: {entry.Value}");
    }
}