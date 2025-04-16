using System.Text.Json;
using System.Text.Json.Serialization;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using logicGP.Tests.Util.ML.ModelBuilder.Configuration;
using Microsoft.ML;

namespace logicGP.Tests.Util;

public class DataHelper
{
    public enum DataSet
    {
        BalanceScale,
        HeartDisease,
        Iris,
        WineQuality,
        BreastCancer,
        AdultIncome,
        CreditCardFraud,
        BanknoteAuthentication
    }

    public static string GenerateModelBuilderConfig(DataSet dataSet,
        string filePath, string labelColumn, int trainingTime,
        string[] trainers)
    {
        var dataSource = new TabularFileDataSourceV3
        {
            EscapeCharacter = '\\',
            ReadMultiLines = false,
            AllowQuoting = false,
            FilePath = filePath,
            Delimiter = ",",
            DecimalMarker = '.',
            HasHeader = true
        };
        var environment = new LocalEnvironmentV1
        {
            Type = "LocalCPU",
            EnvironmentType = EnvironmentType.LocalCPU
        };
        var trainingOption = new ClassificationTrainingOptionV2
        {
            Subsampling = false,
            TrainingTime = trainingTime,
            LabelColumn = labelColumn,
            AvailableTrainers = trainers,
            ValidationOption = new CrossValidationOptionV0
            {
                NumberOfFolds = 10
            }
        };
        var config = new TrainingConfiguration
        {
            Scenario = ScenarioType.Classification,
            DataSource = dataSource,
            Environment = environment,
            TrainingOption = trainingOption
        };
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        return JsonSerializer.Serialize(config, options);
    }

    public static void MakeTrainTestSets(IDataView dataView, string path)
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var cvResults = mlContext.Data.CrossValidationSplit(dataView);
        var index = 0;
        foreach (var fold in cvResults)
        {
            index++;
            var trainData = fold.TrainSet;
            var testData = fold.TestSet;
            var trainDataPath =
                $"{path}_train_{index}.csv";
            var testDataPath =
                $"{path}_test_{index}.csv";
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

    public static Dictionary<string, float> ParseMLRun(string filePath)
    {
        using var reader = new StreamReader(filePath);
        var bestMacroaccuracy = new Dictionary<string, float>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()?.TrimStart();
            if (line[0] != '|') continue;
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

        return bestMacroaccuracy;
    }
}