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
            "/Users/nunkesser/repos/work/articles/logicgp/data/ucimlrepo/carevaluation/car_evaluation_strings.csv",
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
}