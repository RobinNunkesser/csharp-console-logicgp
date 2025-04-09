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
public class CarEvaluationTests : RealTests
{
    private readonly IDataView _data;

    public CarEvaluationTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "car_evaluation_strings.csv");
        _data = mlContext.Data.LoadFromTextFile<CarEvaluationModelInput>(
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
            new LookupMap<string>("unacc"),
            new LookupMap<string>("acc"),
            new LookupMap<string>("good"),
            new LookupMap<string>("vgood")
        };
        var mlContext = new MLContext();
        var testResults = TestFlRw(trainer, _data, lookupData, 10);
        var metrics = mlContext.MulticlassClassification
            .Evaluate(testResults, trainer.Label);

        Assert.IsTrue(metrics.MacroAccuracy > 0.4483);
        Assert.IsTrue(metrics.MacroAccuracy < 0.4484);
    }

    protected override EstimatorChain<ITransformer?> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap)
    {
        var mlContext = new MLContext();
        var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(
                new[]
                {
                    new InputOutputColumnPair(@"buying", @"buying"),
                    new InputOutputColumnPair(@"maint", @"maint"),
                    new InputOutputColumnPair(@"doors", @"doors"),
                    new InputOutputColumnPair(@"persons", @"persons"),
                    new InputOutputColumnPair(@"lug_boot", @"lug_boot"),
                    new InputOutputColumnPair(@"safety", @"safety")
                }, OneHotEncodingEstimator.OutputKind.Key)
            .Append(mlContext.Transforms.Concatenate(@"Features", @"buying",
                @"maint", @"doors", @"persons", @"lug_boot", @"safety"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(@"Label",
                @"class", keyData: lookupIdvMap))
            .Append(trainer);

        return pipeline;
    }

    protected EstimatorChain<ITransformer?> GetGeneratedPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap)
    {
        var mlContext = new MLContext();
        var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(
                new[]
                {
                    new InputOutputColumnPair(@"buying", @"buying"),
                    new InputOutputColumnPair(@"maint", @"maint"),
                    new InputOutputColumnPair(@"doors", @"doors"),
                    new InputOutputColumnPair(@"persons", @"persons"),
                    new InputOutputColumnPair(@"lug_boot", @"lug_boot"),
                    new InputOutputColumnPair(@"safety", @"safety")
                })
            .Append(mlContext.Transforms.Concatenate(@"Features", @"buying",
                @"maint", @"doors", @"persons", @"lug_boot", @"safety"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(@"Label",
                @"class", keyData: lookupIdvMap))
            .Append(trainer);

        return pipeline;
    }
}