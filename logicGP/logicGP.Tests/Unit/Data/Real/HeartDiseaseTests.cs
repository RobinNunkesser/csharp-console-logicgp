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
public class HeartDiseaseTests : RealTests
{
    private readonly IDataView _data;

    public HeartDiseaseTests()
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "Heart_Disease.csv");
        _data = mlContext.Data.LoadFromTextFile<HeartDiseaseModelInput>(
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
            new LookupMap<uint>(0),
            new LookupMap<uint>(1),
            new LookupMap<uint>(2),
            new LookupMap<uint>(3),
            new LookupMap<uint>(4)
        };
        trainer.Classes = lookupData.Length;
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var testResults = TestFlRw(trainer, _data, _data, lookupData, 10);
        var metrics = mlContext.MulticlassClassification
            .Evaluate(testResults, trainer.Label);


        Assert.IsTrue(metrics.MacroAccuracy > 0.24);
        Assert.IsTrue(metrics.MacroAccuracy < 0.25);
    }


    protected override EstimatorChain<ITransformer> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap)
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var pipeline = mlContext.Transforms.ReplaceMissingValues(new[]
            {
                new InputOutputColumnPair(@"age", @"age"),
                new InputOutputColumnPair(@"sex", @"sex"),
                new InputOutputColumnPair(@"cp", @"cp"),
                new InputOutputColumnPair(@"trestbps", @"trestbps"),
                new InputOutputColumnPair(@"chol", @"chol"),
                new InputOutputColumnPair(@"fbs", @"fbs"),
                new InputOutputColumnPair(@"restecg", @"restecg"),
                new InputOutputColumnPair(@"thalach", @"thalach"),
                new InputOutputColumnPair(@"exang", @"exang"),
                new InputOutputColumnPair(@"oldpeak", @"oldpeak"),
                new InputOutputColumnPair(@"slope", @"slope"),
                new InputOutputColumnPair(@"ca", @"ca"),
                new InputOutputColumnPair(@"thal", @"thal")
            })
            .Append(mlContext.Transforms.NormalizeBinning(new[]
            {
                new InputOutputColumnPair(@"age", @"age"),
                new InputOutputColumnPair(@"trestbps", @"trestbps"),
                new InputOutputColumnPair(@"chol", @"chol"),
                new InputOutputColumnPair(@"thalach", @"thalach"),
                new InputOutputColumnPair(@"oldpeak", @"oldpeak"),
                new InputOutputColumnPair(@"ca", @"ca")
            }, maximumBinCount: 4))
            .Append(mlContext.Transforms.Concatenate(@"Features", @"age",
                @"sex", @"cp", @"trestbps", @"chol", @"fbs", @"restecg",
                @"thalach", @"exang", @"oldpeak", @"slope", @"ca", @"thal"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(@"Label",
                @"num", keyData: lookupIdvMap))
            .Append(trainer);

        return pipeline;
    }
}