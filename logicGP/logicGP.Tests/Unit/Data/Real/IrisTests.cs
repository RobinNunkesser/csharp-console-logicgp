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
public class IrisTests : RealTests
{
    private readonly IDataView _data;

    public IrisTests()
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "Iris.csv");
        _data = mlContext.Data.LoadFromTextFile<IrisModelInput>(
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
            new LookupMap<string>("Iris-setosa"),
            new LookupMap<string>("Iris-versicolor"),
            new LookupMap<string>("Iris-virginica")
        };
        trainer.Classes = lookupData.Length;
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var testResults = TestFlRw(trainer, _data, _data, lookupData, 10);
        var metrics = mlContext.MulticlassClassification
            .Evaluate(testResults, trainer.Label);


        Assert.IsTrue(metrics.MacroAccuracy > 0.559);
        Assert.IsTrue(metrics.MacroAccuracy < 0.56);
    }

    protected override EstimatorChain<ITransformer> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap)
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;

        var pipeline = mlContext.Transforms.ReplaceMissingValues(new[]
            {
                new InputOutputColumnPair(@"sepal length", @"sepal length"),
                new InputOutputColumnPair(@"sepal width", @"sepal width"),
                new InputOutputColumnPair(@"petal length", @"petal length"),
                new InputOutputColumnPair(@"petal width", @"petal width")
            })
            .Append(mlContext.Transforms.NormalizeBinning(new[]
            {
                new InputOutputColumnPair(@"sepal length", @"sepal length"),
                new InputOutputColumnPair(@"sepal width", @"sepal width"),
                new InputOutputColumnPair(@"petal length", @"petal length"),
                new InputOutputColumnPair(@"petal width", @"petal width")
            }, maximumBinCount: 4))
            .Append(mlContext.Transforms.Concatenate(@"Features",
                @"sepal length", @"sepal width", @"petal length",
                @"petal width"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(@"Label",
                @"class", keyData: lookupIdvMap))
            .Append(trainer);

        return pipeline;
    }
}