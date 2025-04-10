using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public abstract class
    LogicGpTrainerBase<TTransformer>(
        LogicGpAlgorithm algorithm,
        DataManager data) : IEstimator<ITransformer>


{
    private IIndividual? _chosenIndividual;
    public required string Label { get; set; }
    public required int MaxGenerations { get; set; } = 10000;
    public required int Classes { get; set; } = 2;

    public required Dictionary<int, string> LabelKeyToValueDictionary
    {
        get;
        set;
    }

    public ITransformer Fit(IDataView input)
    {
        var featureNames = input.GetFeaturesSlotNames();
        // Split data into k folds
        const int k = 5; // Number of folds
        var mlContext = new MLContext();
        var cvResults = mlContext.Data.CrossValidationSplit(input);
        var candidates = new IIndividualList[k];
        var foldIndex = 0;

        ParameterizeAlgorithm(algorithm);

        foreach (var fold in cvResults)
        {
            var trainFeatures = fold.TrainSet
                .GetColumn<float[]>(DefaultColumnNames.Features)
                .ToList();
            // Training
            var individuals =
                algorithm.Train(fold.TrainSet, foldIndex == 0, Label,
                    MaxGenerations);
            // Validating
            var validationMetrics =
                algorithm.Validate(fold.TestSet, individuals, Label);
            // Selecting
            var selection = new BestModelForEachSizeSelection();
            candidates[foldIndex++] = selection.Process(individuals);
        }

        // Final Selection
        var bestSelection = new FinalModelSelection();
        var allCandidates = candidates.SelectMany(i => i).ToList();
        var candidatePopulation = new Population();
        foreach (var candidate in allCandidates)
            candidatePopulation.Add(candidate);
        _chosenIndividual = bestSelection.Process(candidatePopulation)[0];
        Console.WriteLine($"Chosen individual: \n{_chosenIndividual}");


        var mapping = new LogicGpMapping(_chosenIndividual);
        if (Classes == 2)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<BinaryClassificationInputSchema,
                        BinaryClassificationBinaryOutputSchema>(),
                null).Fit(input);
        if (Classes == 3)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        TernaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 4)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        QuaternaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 5)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        QuinaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 6)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        SenaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 7)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        SeptenaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 8)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        OctonaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 9)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        NonaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 10)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        DenaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 11)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        UndenaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 12)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        DuodenaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 13)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        TridenaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 14)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        TetradenaryClassificationOutputSchema>(),
                null).Fit(input);
        if (Classes == 15)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<MulticlassClassificationInputSchema,
                        PentadenaryClassificationOutputSchema>(),
                null).Fit(input);
        throw new ArgumentOutOfRangeException(
            $"The number of classes {Classes} is not supported.");
    }

    /// <summary>
    ///     This method cannot be implemented with reasonable effort because
    ///     ML.NET only exposes the necessary API to "best friends".
    /// </summary>
    /// <param name="inputSchema"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public SchemaShape GetOutputSchema(SchemaShape inputSchema)
    {
        var mlContext = new MLContext();
        var mapping = new LogicGpMapping(_chosenIndividual);
        if (Classes == 2)
            return mlContext.Transforms.CustomMapping(
                mapping
                    .GetMapping<BinaryClassificationInputSchema,
                        BinaryClassificationBinaryOutputSchema>(),
                null).GetOutputSchema(inputSchema);

        return mlContext.Transforms.CustomMapping(
            mapping
                .GetMapping<MulticlassClassificationInputSchema,
                    MulticlassClassificationInputSchema>(),
            null).GetOutputSchema(inputSchema);
    }


    protected abstract void ParameterizeAlgorithm(
        LogicGpAlgorithm logicGpAlgorithm);
}