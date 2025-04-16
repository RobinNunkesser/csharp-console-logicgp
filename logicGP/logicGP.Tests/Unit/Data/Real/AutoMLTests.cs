using System.Globalization;
using logicGP.Tests.Util;

namespace logicGP.Tests.Unit.Data.Real;

[TestClass]
public class AutoMLTests
{
    private void ParseMLRunLog(string filePath)
    {
        var bestMacroaccuracy = new Dictionary<string, float>();
        var accuracies = DataHelper.ParseMLRun(filePath);
        UpdateAndFilterAccuracies(accuracies, bestMacroaccuracy);

        PrintAccuracies(bestMacroaccuracy);
    }

    [TestMethod]
    public void SimulateHeartDiseaseMLNet()
    {
        var filePath = "file";
        var labelColumn = "label";
        var trainingTime = 20;
        string[] trainers = ["trainera", "trainerb"];
        var config =
            DataHelper.GenerateModelBuilderConfig(DataHelper.DataSet
                .HeartDisease, filePath, labelColumn, trainingTime, trainers);
        Assert.IsNotNull(config);
        Console.WriteLine(config);
    }

    private void PrintAccuracies(Dictionary<string, float> bestMacroaccuracy)
    {
        foreach (var entry in bestMacroaccuracy)
            Console.WriteLine(
                $"{entry.Key}: {entry.Value.ToString(CultureInfo.InvariantCulture)}");

        Console.WriteLine(
            (bestMacroaccuracy["FastTreeOva"] * 100).ToString(CultureInfo
                .InvariantCulture));
        Console.WriteLine(
            (bestMacroaccuracy["FastForestOva"] * 100).ToString(CultureInfo
                .InvariantCulture));
        Console.WriteLine(
            (bestMacroaccuracy["LbfgsMaximumEntropyMulti"] * 100).ToString(
                CultureInfo.InvariantCulture));
        Console.WriteLine(
            (bestMacroaccuracy["SdcaLogisticRegressionOva"] * 100).ToString(
                CultureInfo.InvariantCulture));
        Console.WriteLine(
            (bestMacroaccuracy["SdcaMaximumEntropyMulti"] * 100).ToString(
                CultureInfo.InvariantCulture));
        Console.WriteLine(
            (bestMacroaccuracy["LbfgsLogisticRegressionOva"] * 100).ToString(
                CultureInfo.InvariantCulture));
    }

    private void UpdateAndFilterAccuracies(Dictionary<string, float> accuracies,
        Dictionary<string, float> bestMacroaccuracy)
    {
        foreach (var accuracy in accuracies.Where(accuracy =>
                     !bestMacroaccuracy.ContainsKey(accuracy.Key) ||
                     bestMacroaccuracy[accuracy.Key] < accuracy.Value))
        {
            if (!accuracy.Key.Equals("FastTreeOva") &&
                !accuracy.Key.Equals("LbfgsMaximumEntropyMulti") &&
                !accuracy.Key.Equals("FastForestOva") &&
                !accuracy.Key.Equals("SdcaLogisticRegressionOva") &&
                !accuracy.Key.Equals("LbfgsLogisticRegressionOva") &&
                !accuracy.Key.Equals("SdcaMaximumEntropyMulti")) continue;
            bestMacroaccuracy[accuracy.Key] = accuracy.Value;
        }
    }
}