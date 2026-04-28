using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Italbytz.AI;
using Italbytz.AI.ML.Core;
using Italbytz.AI.ML.LogicGp;
using Italbytz.AI.ML.UciDatasets;
using Italbytz.AI.ML.Core.Configuration;
using Microsoft.ML;

namespace Italbytz.AI.ML.LogicGp.Benchmark.Cli.Commands;

public static class BenchmarkCommand
{
    public class BenchmarkResult
    {
        [JsonPropertyName("trainer")]
        public string Trainer { get; set; } = string.Empty;

        [JsonPropertyName("dataset")]
        public string Dataset { get; set; } = string.Empty;

        [JsonPropertyName("f1_score")]
        public double F1Score { get; set; }

        [JsonPropertyName("f1_averaging")]
        public string F1Averaging { get; set; } = "macro";

        [JsonPropertyName("train_time_ms")]
        public long TrainTimeMs { get; set; }

        [JsonPropertyName("total_time_ms")]
        public long TotalTimeMs { get; set; }

        [JsonPropertyName("num_rules")]
        public int NumRules { get; set; }

        [JsonPropertyName("num_atoms")]
        public int NumAtoms { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "ok";

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public static int Run(string[] args)
    {
        var options = ParseArgs(args);
        var trainerType = GetOption(options, "trainer", "flcw-macro") ?? "flcw-macro";
        var datasetName = GetOption(options, "dataset", "iris") ?? "iris";
        var generations = GetIntOption(options, "generations", 100);
        var population = GetIntOption(options, "population", 1000);
        var maxTime = GetIntOption(options, "max-time", 60);
        var cvFolds = GetIntOption(options, "cv-folds", 5);
        var seed = GetIntOption(options, "seed", 42);
        var f1Averaging = (GetOption(options, "f1-averaging", "macro") ?? "macro").ToLowerInvariant();
        var outputPath = GetOption(options, "output", null);

        var exitCode = RunBenchmark(trainerType, datasetName, generations, population, maxTime, cvFolds, seed, f1Averaging, outputPath);
        return exitCode;
    }

    private static int RunBenchmark(
        string trainerType,
        string datasetName,
        int generations,
        int population,
        int maxTime,
        int cvFolds,
        int seed,
        string f1Averaging,
        string? outputPath)
    {
        try
        {
            var totalStopwatch = Stopwatch.StartNew();

            var dataset = ResolveDataset(datasetName);
            var trainStopwatch = Stopwatch.StartNew();

            var (metrics, trainer, fittedModel) = TrainAndEvaluate(trainerType, dataset, generations, population, maxTime, cvFolds, seed);

            trainStopwatch.Stop();
            totalStopwatch.Stop();

            var f1Score = ComputeF1(metrics.ConfusionMatrix.Counts, f1Averaging);
            var resolvedModel = ResolveModel(trainer, fittedModel);
            var (numRules, numAtoms) = ExtractModelSize(resolvedModel);

            var result = new BenchmarkResult
            {
                Trainer = trainerType,
                Dataset = datasetName,
                F1Score = f1Score,
                F1Averaging = f1Averaging,
                TrainTimeMs = trainStopwatch.ElapsedMilliseconds,
                TotalTimeMs = totalStopwatch.ElapsedMilliseconds,
                NumRules = numRules,
                NumAtoms = numAtoms,
                Status = "ok",
                Timestamp = DateTime.UtcNow
            };

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(result, jsonOptions);

            if (!string.IsNullOrWhiteSpace(outputPath))
            {
                File.WriteAllText(outputPath!, json);
            }
            else
            {
                Console.WriteLine(json);
            }

            return 0;
        }
        catch (Exception ex)
        {
            var errorResult = new BenchmarkResult
            {
                Trainer = trainerType,
                Dataset = datasetName,
                F1Averaging = f1Averaging,
                Status = "error",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            };

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            Console.Error.WriteLine(JsonSerializer.Serialize(errorResult, jsonOptions));
            return 1;
        }
    }

    private static Dictionary<string, string> ParseArgs(string[] args)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < args.Length; i++)
        {
            var current = args[i];
            if (!current.StartsWith("--", StringComparison.Ordinal))
            {
                continue;
            }

            var key = current[2..];
            var value = i + 1 < args.Length && !args[i + 1].StartsWith("--", StringComparison.Ordinal)
                ? args[++i]
                : "true";
            result[key] = value;
        }

        return result;
    }

    private static string? GetOption(Dictionary<string, string> options, string key, string? defaultValue)
    {
        return options.TryGetValue(key, out var value) ? value : defaultValue;
    }

    private static int GetIntOption(Dictionary<string, string> options, string key, int defaultValue)
    {
        if (!options.TryGetValue(key, out var value))
        {
            return defaultValue;
        }

        return int.TryParse(value, out var parsed) ? parsed : defaultValue;
    }

    private static IDataset ResolveDataset(string datasetName)
    {
        var normalized = NormalizeName(datasetName);
        var dataType = typeof(Data);

        var matchingProperty = dataType
            .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .FirstOrDefault(p => NormalizeName(p.Name) == normalized);

        if (matchingProperty == null)
        {
            var available = string.Join(", ", dataType
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(p => p.Name)
                .OrderBy(n => n));
            throw new ArgumentException($"Unknown dataset '{datasetName}'. Available datasets: {available}");
        }

        var value = matchingProperty.GetValue(null);
        if (value == null)
        {
            throw new InvalidOperationException($"Dataset property '{matchingProperty.Name}' is null.");
        }

        return value as IDataset
               ?? throw new InvalidOperationException(
                   $"Dataset property '{matchingProperty.Name}' does not implement {nameof(IDataset)}.");
    }

    private static (Microsoft.ML.Data.MulticlassClassificationMetrics metrics, IEstimator<ITransformer> trainer, object fittedModel)
        TrainAndEvaluate(string trainerType, IDataset dataset, int generations, int population, int maxTime, int cvFolds, int seed)
    {
        var trainer = CreateTrainer(trainerType, generations, population, maxTime, cvFolds, seed);

        ThreadSafeRandomNetCore.Seed = seed;
        ThreadSafeMLContext.Seed = seed;

        var pipeline = dataset.BuildPipeline(
            ThreadSafeMLContext.LocalMLContext,
            trainer,
            ScenarioType.Classification,
            ProcessingType.FeatureBinningAndCustomLabelMapping);

        var model = pipeline.Fit(dataset.DataView);
        var predictions = model.Transform(dataset.DataView);
        var metrics = ThreadSafeMLContext.LocalMLContext.MulticlassClassification.Evaluate(predictions);

        return (metrics, trainer, model);
    }

    private static IEstimator<ITransformer> CreateTrainer(
        string trainerType,
        int generations,
        int population,
        int maxTime,
        int cvFolds,
        int seed)
    {
        return trainerType.ToLowerInvariant() switch
        {
            "flcw-macro" => new LogicGpFlcwMacroMulticlassTrainer<TernaryClassificationOutput>(generations, cvFolds, maxTime),
            "flcw-micro" => new LogicGpFlcwMicroMulticlassTrainer<TernaryClassificationOutput>(generations, cvFolds, maxTime),
            "rlcw-macro" => new LogicGpRlcwMacroMulticlassTrainer<TernaryClassificationOutput>(
                phase1Time: generations,
                phase2Time: generations,
                maxIndividuals: population),
            "rlcw-micro" => new LogicGpRlcwMicroMulticlassTrainer<TernaryClassificationOutput>(
                phase1Time: generations,
                phase2Time: generations,
                maxIndividuals: population),
            _ => throw new ArgumentException("Unknown trainer '" + trainerType + "'. Supported: flcw-macro, flcw-micro, rlcw-macro, rlcw-micro")
        };
    }

    private static string NormalizeName(string value)
    {
        return new string(value
            .ToLowerInvariant()
            .Where(ch => char.IsLetterOrDigit(ch))
            .ToArray());
    }

    private static double ComputeF1(IReadOnlyList<IReadOnlyList<double>> confusion, string averaging)
    {
        return averaging switch
        {
            "micro" => ComputeMicroF1(confusion),
            _ => ComputeMacroF1(confusion)
        };
    }

    private static double ComputeMacroF1(IReadOnlyList<IReadOnlyList<double>> confusion)
    {
        if (confusion.Count == 0)
        {
            return 0;
        }

        var classCount = confusion.Count;
        var f1Sum = 0.0;

        for (var i = 0; i < classCount; i++)
        {
            var tp = confusion[i][i];
            var fn = confusion[i].Sum() - tp;

            var fp = 0.0;
            for (var r = 0; r < classCount; r++)
            {
                fp += confusion[r][i];
            }

            fp -= tp;

            var precision = tp + fp > 0 ? tp / (tp + fp) : 0.0;
            var recall = tp + fn > 0 ? tp / (tp + fn) : 0.0;
            var f1 = precision + recall > 0 ? 2 * precision * recall / (precision + recall) : 0.0;
            f1Sum += f1;
        }

        return f1Sum / classCount;
    }

    private static double ComputeMicroF1(IReadOnlyList<IReadOnlyList<double>> confusion)
    {
        if (confusion.Count == 0)
        {
            return 0;
        }

        var total = 0.0;
        var tp = 0.0;

        for (var r = 0; r < confusion.Count; r++)
        {
            for (var c = 0; c < confusion[r].Count; c++)
            {
                total += confusion[r][c];
                if (r == c)
                {
                    tp += confusion[r][c];
                }
            }
        }

        if (total <= 0)
        {
            return 0;
        }

        return tp / total;
    }

    private static (int numRules, int numAtoms) ExtractModelSize(object? model)
    {
        if (model == null)
        {
            return (0, 0);
        }

        var genotype = GetPropertyValue(model, "Genotype");
        if (genotype == null)
        {
            return (0, 0);
        }

        var polynomialProperty = genotype.GetType().GetProperty("Polynomial");
        if (polynomialProperty == null)
        {
            return ExtractFromGenotypeSize(genotype);
        }

        var polynomial = polynomialProperty.GetValue(genotype);
        if (polynomial == null)
        {
            return ExtractFromGenotypeSize(genotype);
        }

        var monomialsProperty = polynomial.GetType().GetProperty("Monomials");
        if (monomialsProperty?.GetValue(polynomial) is not IEnumerable monomials)
        {
            return ExtractFromGenotypeSize(genotype);
        }

        var ruleCount = 0;
        var atomCount = 0;

        foreach (var monomial in monomials)
        {
            ruleCount++;
            var literalsProperty = monomial?.GetType().GetProperty("Literals");
            if (literalsProperty?.GetValue(monomial) is IEnumerable literals)
            {
                foreach (var _ in literals)
                {
                    atomCount++;
                }
            }
        }

        if (ruleCount == 0 && atomCount == 0)
        {
            return ExtractFromGenotypeSize(genotype);
        }

        return (ruleCount, atomCount);
    }

    private static (int numRules, int numAtoms) ExtractFromGenotypeSize(object genotype)
    {
        var sizeProperty = genotype.GetType().GetProperty("Size");
        if (sizeProperty?.GetValue(genotype) is int size && size > 0)
        {
            return (1, size);
        }

        return (0, 0);
    }

    private static object? ResolveModel(object trainer, object fittedModel)
    {
        var model = GetPropertyValue(trainer, "Model");
        if (model != null)
        {
            return model;
        }

        if (GetPropertyValue(trainer, "FinalPopulation") is IEnumerable population)
        {
            var best = SelectBestIndividual(population);
            if (best != null)
            {
                return best;
            }
        }

        return FindModelInObjectGraph(fittedModel, 3);
    }

    private static object? SelectBestIndividual(IEnumerable population)
    {
        object? best = null;

        foreach (var candidate in population)
        {
            if (candidate == null)
            {
                continue;
            }

            if (best == null)
            {
                best = candidate;
                continue;
            }

            if (IsBetter(candidate, best))
            {
                best = candidate;
            }
        }

        return best;
    }

    private static bool IsBetter(object candidate, object incumbent)
    {
        var candidateFitness = GetPropertyValue(candidate, "LatestKnownFitness");
        var incumbentFitness = GetPropertyValue(incumbent, "LatestKnownFitness");

        if (candidateFitness is IComparable candidateComparable && incumbentFitness != null)
        {
            try
            {
                if (candidateComparable.CompareTo(incumbentFitness) > 0)
                {
                    return true;
                }
            }
            catch
            {
            }
        }

        return GetIntPropertyValue(candidate, "Size") > GetIntPropertyValue(incumbent, "Size");
    }

    private static object? FindModelInObjectGraph(object? root, int maxDepth)
    {
        if (root == null || maxDepth < 0)
        {
            return null;
        }

        if (LooksLikeIndividual(root))
        {
            return root;
        }

        var type = root.GetType();

        var modelProperty = type.GetProperty("Model", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (modelProperty?.GetValue(root) is { } modelFromProperty && LooksLikeIndividual(modelFromProperty))
        {
            return modelFromProperty;
        }

        foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
        {
            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            object? value;
            try
            {
                value = property.GetValue(root);
            }
            catch
            {
                continue;
            }

            if (ReferenceEquals(value, null))
            {
                continue;
            }

            if (LooksLikeIndividual(value))
            {
                return value;
            }

            if (value is string)
            {
                continue;
            }

            var nested = FindModelInObjectGraph(value, maxDepth - 1);
            if (nested != null)
            {
                return nested;
            }
        }

        foreach (var field in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
        {
            var value = field.GetValue(root);
            if (value != null && LooksLikeIndividual(value))
            {
                return value;
            }

            if (value == null || value is string)
            {
                continue;
            }

            var nested = FindModelInObjectGraph(value, maxDepth - 1);
            if (nested != null)
            {
                return nested;
            }
        }

        return null;
    }

    private static object? GetPropertyValue(object source, string propertyName)
    {
        return source.GetType().GetProperty(propertyName)?.GetValue(source);
    }

    private static int GetIntPropertyValue(object source, string propertyName)
    {
        return GetPropertyValue(source, propertyName) is int value ? value : 0;
    }

    private static bool LooksLikeIndividual(object value)
    {
        var type = value.GetType();
        return type.GetProperty("Genotype") != null &&
               type.GetProperty("LatestKnownFitness") != null &&
               type.GetProperty("Size") != null;
    }
}