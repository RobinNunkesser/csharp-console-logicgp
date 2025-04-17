using System.Diagnostics;
using System.Globalization;
using Italbytz.Adapters.Algorithms.AI.Learning.ML;
using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using logicGP.Tests.Data.Real;
using logicGP.Tests.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

public abstract class RealTests
{
    protected int[]
        Seeds =
        [
            42, 7, 13, 23, 3, 666, 777, 9, 17, 21
        ]; //[42, 23, 7, 3, 99, 1, 0, 8, 15, 16];

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

    protected void SaveTrainTestSplit(
        IDataView data, string fileName)
    {
        foreach (var mlSeed in Seeds)
        {
            var mlContext = new MLContext(mlSeed);
            var trainTestSplit = mlContext.Data.TrainTestSplit(data, 0.2);
            var trainSet = trainTestSplit.TrainSet;
            var testSet = trainTestSplit.TestSet;
            var dataFolder =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            trainSet.SaveAsCsv(Path.Combine(dataFolder,
                $"{fileName}_seed_{mlSeed}_train.csv"));
            testSet.SaveAsCsv(Path.Combine(dataFolder,
                $"{fileName}_seed_{mlSeed}_test.csv"));
        }
    }

    protected void SimulateMLNetOnAllTrainers(DataHelper.DataSet dataSet,
        string relativeDataPath,
        string filePrefix, string labelColumn, int trainingTime)
    {
        LogWriter = new StreamWriter(LogFile);
        // LGBM is not available (at least on macOS-ARM and linux-x86)
        string[] availableTrainers =
        [
            "LBFGS", "FASTFOREST", "SDCA", "FASTTREE"
        ];
        foreach (var trainer in availableTrainers)
        {
            var bestAccuracy = 0.0;
            foreach (var seed in Seeds)
            {
                var trainingData = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    relativeDataPath,
                    $"{filePrefix}_seed_{seed}_train.csv");
                var testData = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    relativeDataPath,
                    $"{filePrefix}_seed_{seed}_test.csv");
                string[] trainers = [trainer];
                var macroAccuracy = SimulateMLNet(
                    dataSet,
                    trainingData, testData,
                    labelColumn, trainingTime, trainers);
                Console.WriteLine($"{trainer}: {macroAccuracy}");
                Console.Clear();
                if (macroAccuracy > bestAccuracy)
                    bestAccuracy = macroAccuracy;
            }

            Console.WriteLine($"{trainer}: {bestAccuracy}");
            Console.Clear();
        }
    }

    protected void SaveCvSplit(
        IDataView data, string fileName)
    {
        var mlContext = ThreadSafeMLContext.LocalMLContext;
        var cvResults = mlContext.Data.CrossValidationSplit(data);
        var foldIndex = 0;

        foreach (var fold in cvResults)
        {
            var trainSet = fold.TrainSet;
            var testSet = fold.TestSet;
            var dataFolder =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            trainSet.SaveAsCsv(Path.Combine(dataFolder,
                $"{fileName}_fold_{foldIndex}_train.csv"));
            testSet.SaveAsCsv(Path.Combine(dataFolder,
                $"{fileName}_fold_{foldIndex}_test.csv"));
            foldIndex++;
        }
    }

    protected LogicGpFlrwMacroMulticlassTrainer GetFlRwMacroTrainer(int classes)
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider
                .GetRequiredService<LogicGpFlrwMacroMulticlassTrainer>();
        trainer.Classes = classes;
        return trainer;
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

        foreach (var mlSeed in Seeds)
        foreach (var randomSeed in Seeds)
        {
            var mlContext = new MLContext(mlSeed);
            ThreadSafeRandomNetCore.Seed = randomSeed;
            var trainTestSplit = mlContext.Data.TrainTestSplit(data, 0.2);

            var trainData = trainTestSplit.TrainSet;
            var testData = trainTestSplit.TestSet;

            var mlModel = Train(mlContext, trainer, trainData, lookupData,
                generations);
            Assert.IsNotNull(mlModel);
            var chosenIndividual =
                ((LogicGpTrainerBase<ITransformer>)trainer)
                .ChosenIndividual;
            var testResults = mlModel.Transform(testData);
            LogWriter?.WriteLine(
                $"ML seed: {mlSeed}, Random seed: {randomSeed}, Generations: {generations}");
            LogWriter?.WriteLine();
            LogWriter?.Write(chosenIndividual.ToString());
            var metrics = new MLContext().MulticlassClassification
                .Evaluate(testResults);
            LogWriter?.WriteLine();
            LogWriter?.WriteLine(
                $"MacroAccuracy: {metrics.MacroAccuracy.ToString(CultureInfo.InvariantCulture)}");
            ResultWriter?.WriteLine(
                metrics.MacroAccuracy.ToString(CultureInfo
                    .InvariantCulture));
            LogWriter?.Flush();
            ResultWriter?.Flush();
            /*ResultWriter?.WriteLine(
                    chosenIndividual.LatestKnownFitness[0].ToString(
                        CultureInfo.InvariantCulture));*/
        }

        LogWriter?.Close();
        ResultWriter?.Close();
    }

    protected abstract EstimatorChain<ITransformer?> GetPipeline(
        LogicGpTrainerBase<ITransformer> trainer, IDataView lookupIdvMap);

    private void ParseMLRunLog(string filePath)
    {
        var bestMacroaccuracy = new Dictionary<string, float>();
        var accuracies = DataHelper.ParseMLRun(filePath);
        UpdateAndFilterAccuracies(accuracies, bestMacroaccuracy);

        PrintAccuracies(bestMacroaccuracy);
    }

    protected double SimulateMLNet(DataHelper.DataSet dataSet,
        string trainingData,
        string testData,
        string labelColumn, int trainingTime,
        string[] trainers)
    {
        // Configure a Model Builder configuration
        var config =
            DataHelper.GenerateModelBuilderConfig(dataSet, trainingData,
                labelColumn, trainingTime, trainers);
        Assert.IsNotNull(config);
        // Save the configuration
        var modelFileName = trainingData
            .Substring(trainingData.LastIndexOf('/') + 1)
            .Replace("train.csv", "");
        modelFileName = $"{modelFileName}{trainers[0]}";
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            $"{modelFileName}.mbconfig");
        File.WriteAllText(configPath, config);
        // Run AutoML
        RunAutoMLForConfig(modelFileName);
        CleanUp();
        var modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            $"{modelFileName}.mlnet");
        var mlContext = new MLContext();
        try
        {
            var mlModel = mlContext.Model.Load(modelPath, out _);

            var testDataView = dataSet switch
            {
                DataHelper.DataSet.HeartDisease => mlContext.Data
                    .LoadFromTextFile<HeartDiseaseModelInputOriginal>(
                        testData,
                        ',', true),
                DataHelper.DataSet.Iris => mlContext.Data
                    .LoadFromTextFile<IrisModelInput>(
                        testData,
                        ',', true),
                _ => throw new ArgumentOutOfRangeException(nameof(dataSet),
                    dataSet,
                    null)
            };
            var testResult = mlModel.Transform(testDataView);
            var metrics = mlContext.MulticlassClassification
                .Evaluate(testResult, labelColumn);
            return metrics.MacroAccuracy;
        }
        catch (Exception e)
        {
            Console.WriteLine(
                $"Error loading model for data set {trainingData} and trainer {trainers[0]}.");
        }

        return 0.0;
    }

    private void CleanUp()
    {
        var directory = AppDomain.CurrentDomain.BaseDirectory;
        var projectFiles = Directory.GetFiles(directory, "*.cs");
        foreach (var file in projectFiles) File.Delete(file);
        projectFiles = Directory.GetFiles(directory, "*.csproj");
        foreach (var file in projectFiles) File.Delete(file);
    }


    private void RunAutoMLForConfig(string modelFileName)
    {
        var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var mlnet = new Process();
        mlnet.StartInfo.FileName = "mlnet";
        mlnet.StartInfo.WorkingDirectory =
            AppDomain.CurrentDomain.BaseDirectory;
        //mlnet.StartInfo.Arguments = $"train --training-config config.mbconfig --log-file-path {timeStamp}.log";
        //mlnet.StartInfo.Arguments = "train --training-config config.mbconfig";
        mlnet.StartInfo.Arguments =
            $"train --training-config {modelFileName}.mbconfig -v q";
        mlnet.Start();
        mlnet.WaitForExit();
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