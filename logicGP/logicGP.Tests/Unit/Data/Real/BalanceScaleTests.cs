using System.Globalization;
using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util;
using logicGP.Tests.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class BalanceScaleTests : RealTests
{
    private readonly IDataView _data;

    public BalanceScaleTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "balancescale.csv");
        _data = mlContext.Data.LoadFromTextFile<BalanceScaleModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        ThreadSafeRandomNetCore.Seed = 42;
        var accuracy = TestFlRw(_data, "class", 10);
        Assert.IsTrue(accuracy > 0.4644);
        Assert.IsTrue(accuracy < 0.4645);
    }

    [TestMethod]
    public void TestWithFullPipeline()
    {
        var mlContext = new MLContext();
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMacroMulticlassTrainer>();
        trainer.Label = "class";
        trainer.MaxGenerations = 10;
        var pipeline = mlContext.Transforms.ReplaceMissingValues(new[]
            {
                new InputOutputColumnPair(@"right-distance", @"right-distance"),
                new InputOutputColumnPair(@"right-weight", @"right-weight"),
                new InputOutputColumnPair(@"left-distance", @"left-distance"),
                new InputOutputColumnPair(@"left-weight", @"left-weight")
            })
            .Append(mlContext.Transforms.Concatenate(@"Features",
                @"right-distance", @"right-weight", @"left-distance",
                @"left-weight")).Append(trainer);
        var mlModel = pipeline.Fit(_data);
        Assert.IsNotNull(mlModel);

        var testResults = mlModel.Transform(_data);
        var metrics = mlContext.MulticlassClassification
            .Evaluate(testResults, trainer.Label);
        var acc = metrics.MacroAccuracy.ToString(CultureInfo.InvariantCulture);
    }
}