using System.Globalization;
using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

public abstract class RealTests
{
    protected string? LogFile { get; set; } = null;
    protected StreamWriter? LogWriter { get; set; }
    protected StreamWriter? ResultWriter { get; set; }

    private TransformerChain<ITransformer?>? Train<TLabel>(MLContext mlContext,
        IEstimator<ITransformer> generalTrainer, IDataView trainData,
        LookupMap<TLabel>[] lookupData, int generations = 100)
    {
        var lookupIdvMap = mlContext.Data.LoadFromEnumerable(lookupData);
        if (generalTrainer is not LogicGpTrainerBase<ITransformer> trainer)
            throw new ArgumentException(
                "The trainer must be of type LogicGpTrainerBase<ITransformer>");
        trainer.LabelKeyToValueDictionary =
            LookupMap<TLabel>.KeyToValueMap(lookupData);
        trainer.Label = "Label";
        trainer.MaxGenerations = generations;
        var pipeline = GetPipeline(trainer, lookupIdvMap);
        return pipeline.Fit(trainData);
    }

    protected IDataView TestFlRw<TLabel>(
        IEstimator<ITransformer> generalTrainer,
        IDataView trainData, IDataView testData,
        LookupMap<TLabel>[] lookupData, int generations = 100)
    {
        var mlContext = new MLContext();
        var mlModel = Train(mlContext, generalTrainer, trainData, lookupData,
            generations);
        Assert.IsNotNull(mlModel);
        return mlModel.Transform(testData);
    }

    public void SimulateFlRw<TLabel>(IEstimator<ITransformer> trainer,
        IDataView data, LookupMap<TLabel>[] lookupData, int generations = 10000)
    {
        var logFolder = AppDomain.CurrentDomain.BaseDirectory;
        var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        if (LogFile != null)
        {
            var logPath = Path.Combine(logFolder,
                $"{LogFile}_{timeStamp}.log");
            LogWriter = new StreamWriter(logPath);
            var resultPath = Path.Combine(logFolder,
                $"{LogFile}_{timeStamp}.csv");
            ResultWriter = new StreamWriter(resultPath);
        }

        var seeds = new[] { 42, 23, 7, 3, 99, 1, 0, 8, 15, 16 };
        foreach (var seed in seeds)
        {
            var mlContext = new MLContext(seed);
            var trainTestSplit = mlContext.Data.TrainTestSplit(data, 0.2);
            var trainData = trainTestSplit.TrainSet;
            var mlModel = Train(mlContext, trainer, trainData, lookupData,
                generations);
            Assert.IsNotNull(mlModel);
            var testResults = mlModel.Transform(trainTestSplit.TestSet);
            LogWriter?.WriteLine(
                $"Seed: {seed}, Generations: {generations}");
            LogWriter?.WriteLine();
            LogWriter?.Write(((LogicGpTrainerBase<ITransformer>)trainer)
                .ChosenIndividual?.ToString());
            var metrics = new MLContext().MulticlassClassification
                .Evaluate(testResults);
            LogWriter?.WriteLine();
            LogWriter?.WriteLine(
                $"MacroAccuracy: {metrics.MacroAccuracy.ToString(CultureInfo.InvariantCulture)}");
            ResultWriter?.WriteLine(
                metrics.MacroAccuracy.ToString(CultureInfo.InvariantCulture));
        }

        LogWriter?.Close();
        ResultWriter?.Close();
    }


    protected abstract EstimatorChain<ITransformer?> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap);

    protected double TestFlRw(IDataView data, string label,
        int generations = 100)
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMacroMulticlassTrainer>();
        trainer.Label = label;
        // This is only for testing purposes, in production this should be set to a higher value (e.g. 10000)
        trainer.MaxGenerations = generations;

        var columnData = DataViewExtensions
            .GetColumnAsString(data, trainer.Label).ToList();
        var uniqueValues =
            new HashSet<string>(
                columnData);
        var labels = uniqueValues.OrderBy(c => c).ToList();
        var bestAccuracy = 0.0;
        for (var j = 0; j < 10; j++)
        {
            var mlModel = trainer.Fit(data);
            Assert.IsNotNull(mlModel);
            var testResults = mlModel.Transform(data);
            var trueValues = testResults.GetColumn<string>("y").ToArray();
            var predictedValues =
                testResults.GetColumn<string>("PredictedLabel").ToList();

            var accuracies = new float[labels.Count];
            var counts = new int[labels.Count];

            for (var i = 0; i < predictedValues.Count; i++)
            {
                counts[labels.IndexOf(trueValues[i])]++;
                if (predictedValues[i] == trueValues[i])
                    accuracies[labels.IndexOf(trueValues[i])]++;
            }

            for (var i = 0; i < labels.Count; i++)
            {
                accuracies[i] /= counts[i];
                Console.WriteLine($"{labels[i]}: {accuracies[i]}");
            }

            var macroAccuracy = accuracies.Sum() / labels.Count;
            if (macroAccuracy > bestAccuracy) bestAccuracy = macroAccuracy;
            Console.WriteLine($"MacroAccuracy: {macroAccuracy}");
        }

        Console.WriteLine($"Best MacroAccuracy: {bestAccuracy}");
        return bestAccuracy;
    }
}