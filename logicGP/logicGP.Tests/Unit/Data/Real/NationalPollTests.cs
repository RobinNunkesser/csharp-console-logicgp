using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using logicGP.Tests.Data.Real;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

[TestClass]
public class NationalPollTests : RealTests
{
    private readonly IDataView _data;

    public NationalPollTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "national_poll_on_healthy_aging_npha.csv");
        _data = mlContext.Data.LoadFromTextFile<NationalPollModelInput>(
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
            new LookupMap<uint>(1),
            new LookupMap<uint>(2),
            new LookupMap<uint>(3)
        };
        trainer.Classes = lookupData.Length;
        var mlContext = new MLContext();
        var testResults = TestFlRw(trainer, _data, _data, lookupData, 10);
        var metrics = mlContext.MulticlassClassification
            .Evaluate(testResults, trainer.Label);

        Assert.IsTrue(metrics.MacroAccuracy > 0.358);
        Assert.IsTrue(metrics.MacroAccuracy < 0.359);
    }

    protected override EstimatorChain<ITransformer?> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap)
    {
        var mlContext = new MLContext();
        var pipeline = mlContext.Transforms.ReplaceMissingValues(new[]
            {
                new InputOutputColumnPair(@"Age", @"Age"),
                new InputOutputColumnPair(@"Physical_Health",
                    @"Physical_Health"),
                new InputOutputColumnPair(@"Mental_Health", @"Mental_Health"),
                new InputOutputColumnPair(@"Dental_Health", @"Dental_Health"),
                new InputOutputColumnPair(@"Employment", @"Employment"),
                new InputOutputColumnPair(@"Stress_Keeps_Patient_from_Sleeping",
                    @"Stress_Keeps_Patient_from_Sleeping"),
                new InputOutputColumnPair(
                    @"Medication_Keeps_Patient_from_Sleeping",
                    @"Medication_Keeps_Patient_from_Sleeping"),
                new InputOutputColumnPair(@"Pain_Keeps_Patient_from_Sleeping",
                    @"Pain_Keeps_Patient_from_Sleeping"),
                new InputOutputColumnPair(
                    @"Bathroom_Needs_Keeps_Patient_from_Sleeping",
                    @"Bathroom_Needs_Keeps_Patient_from_Sleeping"),
                new InputOutputColumnPair(@"Uknown_Keeps_Patient_from_Sleeping",
                    @"Uknown_Keeps_Patient_from_Sleeping"),
                new InputOutputColumnPair(@"Trouble_Sleeping",
                    @"Trouble_Sleeping"),
                new InputOutputColumnPair(@"Prescription_Sleep_Medication",
                    @"Prescription_Sleep_Medication"),
                new InputOutputColumnPair(@"Race", @"Race"),
                new InputOutputColumnPair(@"Gender", @"Gender")
            })
            .Append(mlContext.Transforms.Concatenate(@"Features", @"Age",
                @"Physical_Health", @"Mental_Health", @"Dental_Health",
                @"Employment", @"Stress_Keeps_Patient_from_Sleeping",
                @"Medication_Keeps_Patient_from_Sleeping",
                @"Pain_Keeps_Patient_from_Sleeping",
                @"Bathroom_Needs_Keeps_Patient_from_Sleeping",
                @"Uknown_Keeps_Patient_from_Sleeping", @"Trouble_Sleeping",
                @"Prescription_Sleep_Medication", @"Race", @"Gender"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(
                @"Label", @"Number_of_Doctors_Visited",
                keyData: lookupIdvMap))
            .Append(trainer);

        return pipeline;
    }
}