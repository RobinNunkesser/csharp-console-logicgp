using System.Text;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
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

    private const string dataSetPreamble = """
                                           {
                                           "Scenario": "Classification",
                                           "DataSource": {
                                             "Version": 3,
                                             "EscapeCharacter": "\"",
                                             "ReadMultiLines": false,
                                             "AllowQuoting": false,
                                             "Type": "TabularFile",
                                             "FilePath": 
                                           """;

    public static void GenerateModelBuilderConfig(DataSet dataSet,
        string path)
    {
        var sb = new StringBuilder();
        sb.Append(dataSetPreamble);
        File.WriteAllText(path, sb.ToString());
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