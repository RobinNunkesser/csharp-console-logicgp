using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public abstract class
    LogicGpTrainerBase<TTransformer>(
        LogicGpAlgorithm algorithm,
        DataManager data) : IEstimator<TTransformer>
    where TTransformer : ITransformer

{
    public required string Label { get; set; }
    public required int MaxGenerations { get; set; } = 10000;

    public TTransformer Fit(IDataView input)
    {
        // Split data into k folds
        const int k = 5; // Number of folds
        var mlContext = new MLContext();
        var cvResults = mlContext.Data.CrossValidationSplit(input);
        var candidates = new IIndividualList[k];
        var foldIndex = 0;

        ParameterizeAlgorithm(algorithm);

        foreach (var fold in cvResults)
        {
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
        var chosenIndividual = bestSelection.Process(candidatePopulation)[0];
        Console.WriteLine($"Chosen individual: \n{chosenIndividual}");
        var transformer = CreateTransformer(chosenIndividual, data);

        return transformer;
    }

    public SchemaShape GetOutputSchema(SchemaShape inputSchema)
    {
        throw new NotImplementedException();
    }

    protected abstract TTransformer CreateTransformer(
        IIndividual chosenIndividual, DataManager dataManager);

    protected abstract void ParameterizeAlgorithm(
        LogicGpAlgorithm logicGpAlgorithm);
}