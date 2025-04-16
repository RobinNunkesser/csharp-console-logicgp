using Italbytz.Adapters.Algorithms.AI.Learning.ML;
using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using logicGP.Tests.Data.Real;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests.Unit.Data.Real;

[TestClass]
public class WineQualityTests : RealTests
{
    private readonly IDataView _data;

    public WineQualityTests()
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real/WineQuality", "Wine_Quality.csv");
        _data = mlContext.Data.LoadFromTextFile<WineQualityModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        ThreadSafeRandomNetCore.Seed = 42;

        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMacroMulticlassTrainer>();

        var lookupData = new[]
        {
            //new LookupMap<uint>(0),
            //new LookupMap<uint>(1),
            //new LookupMap<uint>(2),
            new LookupMap<uint>(3),
            new LookupMap<uint>(4),
            new LookupMap<uint>(5),
            new LookupMap<uint>(6),
            new LookupMap<uint>(7),
            new LookupMap<uint>(8),
            new LookupMap<uint>(9)
            //new LookupMap<uint>(10)
        };
        trainer.Classes = lookupData.Length;
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var testResults = TestFlRw(trainer, _data, _data, lookupData, 10);
        var metrics = mlContext.MulticlassClassification
            .Evaluate(testResults, trainer.Label);


        Assert.IsTrue(metrics.MacroAccuracy > 0.16);
        Assert.IsTrue(metrics.MacroAccuracy < 0.17);
    }


    protected override EstimatorChain<ITransformer> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap)
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var pipeline = mlContext.Transforms.ReplaceMissingValues(new[]
            {
                new InputOutputColumnPair(@"fixed_acidity", @"fixed_acidity"),
                new InputOutputColumnPair(@"volatile_acidity",
                    @"volatile_acidity"),
                new InputOutputColumnPair(@"citric_acid", @"citric_acid"),
                new InputOutputColumnPair(@"residual_sugar", @"residual_sugar"),
                new InputOutputColumnPair(@"chlorides", @"chlorides"),
                new InputOutputColumnPair(@"free_sulfur_dioxide",
                    @"free_sulfur_dioxide"),
                new InputOutputColumnPair(@"total_sulfur_dioxide",
                    @"total_sulfur_dioxide"),
                new InputOutputColumnPair(@"density", @"density"),
                new InputOutputColumnPair(@"pH", @"pH"),
                new InputOutputColumnPair(@"sulphates", @"sulphates"),
                new InputOutputColumnPair(@"alcohol", @"alcohol")
            })
            .Append(mlContext.Transforms.NormalizeBinning(new[]
            {
                new InputOutputColumnPair(@"fixed_acidity", @"fixed_acidity"),
                new InputOutputColumnPair(@"volatile_acidity",
                    @"volatile_acidity"),
                new InputOutputColumnPair(@"citric_acid", @"citric_acid"),
                new InputOutputColumnPair(@"residual_sugar", @"residual_sugar"),
                new InputOutputColumnPair(@"chlorides", @"chlorides"),
                new InputOutputColumnPair(@"free_sulfur_dioxide",
                    @"free_sulfur_dioxide"),
                new InputOutputColumnPair(@"total_sulfur_dioxide",
                    @"total_sulfur_dioxide"),
                new InputOutputColumnPair(@"density", @"density"),
                new InputOutputColumnPair(@"pH", @"pH"),
                new InputOutputColumnPair(@"sulphates", @"sulphates"),
                new InputOutputColumnPair(@"alcohol", @"alcohol")
            }, maximumBinCount: 4))
            .Append(mlContext.Transforms.Concatenate(@"Features",
                @"fixed_acidity", @"volatile_acidity", @"citric_acid",
                @"residual_sugar", @"chlorides", @"free_sulfur_dioxide",
                @"total_sulfur_dioxide", @"density", @"pH", @"sulphates",
                @"alcohol"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(@"Label",
                @"quality", keyData: lookupIdvMap))
            .Append(trainer);

        return pipeline;
    }
}