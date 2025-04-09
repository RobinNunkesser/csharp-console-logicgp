using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using logicGP.Tests.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace logicGP.Tests;

[TestClass]
public class SolarflareTests : RealTests
{
    private readonly IDataView _data;

    public SolarflareTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "solarflare_1.csv");
        _data = mlContext.Data.LoadFromTextFile<SolarflareModelInput>(
            path,
            ',', true);
    }

    // ToDo: Due to the restructuring of the code, one hot encoding is not supported at the moment.
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
            new LookupMap<uint>(0),
            new LookupMap<uint>(1),
            new LookupMap<uint>(2),
            new LookupMap<uint>(3),
            new LookupMap<uint>(4),
            new LookupMap<uint>(5),
            new LookupMap<uint>(6),
            new LookupMap<uint>(7),
            new LookupMap<uint>(8)
        };
        trainer.Classes = lookupData.Length;
        var mlContext = new MLContext();
        var testResults = TestFlRw(trainer, _data, lookupData, 10);
        var metrics = mlContext.MulticlassClassification
            .Evaluate(testResults, trainer.Label);

        Assert.IsTrue(metrics.MacroAccuracy > 0.2238);
        Assert.IsTrue(metrics.MacroAccuracy < 0.2239);
    }

    protected override EstimatorChain<ITransformer?> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap)
    {
        var mlContext = new MLContext();
        var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(
                new[]
                {
                    new InputOutputColumnPair(@"modified Zurich class",
                        @"modified Zurich class"),
                    new InputOutputColumnPair(@"spot distribution",
                        @"spot distribution")
                }, OneHotEncodingEstimator.OutputKind.Binary)
            .Append(mlContext.Transforms.ReplaceMissingValues(new[]
            {
                new InputOutputColumnPair(@"activity", @"activity"),
                new InputOutputColumnPair(@"evolution", @"evolution"),
                new InputOutputColumnPair(@"previous 24 hour flare activity",
                    @"previous 24 hour flare activity"),
                new InputOutputColumnPair(@"historically-complex",
                    @"historically-complex"),
                new InputOutputColumnPair(@"became complex on this pass",
                    @"became complex on this pass"),
                new InputOutputColumnPair(@"area", @"area"),
                new InputOutputColumnPair(@"area of largest spot",
                    @"area of largest spot")
            }))
            .Append(mlContext.Transforms.Text.FeaturizeText(
                inputColumnName: @"largest spot size",
                outputColumnName: @"largest spot size"))
            .Append(mlContext.Transforms.Concatenate(@"Features",
                @"modified Zurich class", @"spot distribution", @"activity",
                @"evolution", @"previous 24 hour flare activity",
                @"historically-complex", @"became complex on this pass",
                @"area", @"area of largest spot", @"largest spot size"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(@"Label",
                @"flares", keyData: lookupIdvMap))
            .Append(trainer);

        return pipeline;
    }
}