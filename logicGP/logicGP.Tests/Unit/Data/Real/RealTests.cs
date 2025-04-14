using System.Globalization;
using Italbytz.Adapters.Algorithms.AI.Learning.ML;
using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

public abstract class RealTests
{
    protected int[] Seeds = [42, 23, 7, 3, 99, 1, 0, 8, 15, 16];
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
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var mlModel = Train(mlContext, generalTrainer, trainData, lookupData,
            generations);
        Assert.IsNotNull(mlModel);
        return mlModel.Transform(testData);
    }

    public void SimulateFlRw<TLabel>(IEstimator<ITransformer> trainer,
        IDataView data, LookupMap<TLabel>[] lookupData, int generations = 10000)
    {
        var logFolder =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        if (!Directory.Exists(logFolder))
            Directory.CreateDirectory(logFolder);
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

        foreach (var seed in Seeds)
        {
            // Same CV-split, different randomization in GP for every run
            var mlContext =
                ThreadSafeMLContext.LocalMLContext; //new MLContext(seed);
            ThreadSafeRandomNetCore.Seed = seed;
            //var trainTestSplit = mlContext.Data.TrainTestSplit(data, 0.2);

            var trainData = data; //trainTestSplit.TrainSet;
            var testData = data; //trainTestSplit.TestSet;
            if (LogFile != null)
            {
                //trainData.SaveAsCsv($"{LogFile}_seed_{seed}_train.csv");
                //testData.SaveAsCsv($"{LogFile}_seed_{seed}_test.csv");
            }

            var mlModel = Train(mlContext, trainer, trainData, lookupData,
                generations);
            Assert.IsNotNull(mlModel);
            var chosenIndividual =
                ((LogicGpTrainerBase<ITransformer>)trainer).ChosenIndividual;
            var testResults = mlModel.Transform(testData);
            LogWriter?.WriteLine(
                $"Seed: {seed}, Generations: {generations}");
            LogWriter?.WriteLine();
            LogWriter?.Write(chosenIndividual.ToString());
            var metrics = new MLContext().MulticlassClassification
                .Evaluate(testResults);
            LogWriter?.WriteLine();
            LogWriter?.WriteLine(
                $"MacroAccuracy: {metrics.MacroAccuracy.ToString(CultureInfo.InvariantCulture)}");
            /*ResultWriter?.WriteLine(
                metrics.MacroAccuracy.ToString(CultureInfo.InvariantCulture));*/
            ResultWriter?.WriteLine(
                chosenIndividual.LatestKnownFitness[0].ToString(
                    CultureInfo.InvariantCulture));
        }

        LogWriter?.Close();
        ResultWriter?.Close();
    }

    protected abstract EstimatorChain<ITransformer?> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap);
}