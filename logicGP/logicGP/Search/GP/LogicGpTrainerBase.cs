using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public abstract class
    LogicGpTrainerBase<TTransformer>(
        LogicGpAlgorithm algorithm,
        DataManager data) : IEstimator<TTransformer>
    where TTransformer : ITransformer

{
    public required string Label { get; set; }

    public TTransformer Fit(IDataView input)
    {
        // Split data into k folds
        const int k = 5; // Number of folds
        var mlContext = new MLContext();
        var cvResults = mlContext.Data.CrossValidationSplit(input);
        var candidates = new IIndividualList[k];
        var foldIndex = 0;
        data.Initialize(cvResults[0].TrainSet, Label);

        foreach (var fold in cvResults)
        {
            // Training
            if (foldIndex > 0)
                foreach (var literal in data.Literals)
                    literal.GeneratePredictions(
                        fold.TrainSet.GetColumn<float>(literal.Label).ToList());
            var individuals = algorithm.Fit(fold.TrainSet);
            // Testing
            foreach (var literal in data.Literals)
                literal.GeneratePredictions(
                    fold.TestSet.GetColumn<float>(literal.Label).ToList());
            var fitness = new LogicGpPareto
            {
                LabelColumnName = Label,
                Labels = data.Labels
            };
            foreach (var individual in individuals)
            {
                ((LogicGpGenotype)individual.Genotype)
                    .UpdatePredictionsRecursively();
                individual.Generation = 0;
                var fitnessValue = ((IFitnessFunction)fitness).Evaluate(
                    individual,
                    fold.TestSet);
                var accuracy = 0.0;
                for (var i = 0; i < fitnessValue.Length - 1; i++)
                    accuracy += fitnessValue[i];
                individual.LatestKnownFitness =
                    [accuracy, fitnessValue[^1]];
                //individual.LatestKnownFitness = fitnessValue;
            }

            // Selecting
            //candidates[foldIndex++] = individuals;
            var selection = new BestModelForEachSizeSelection();
            candidates[foldIndex++] = selection.Process(individuals);
        }

        var bestSelection = new FinalModelSelection();
        var allCandidates = candidates.SelectMany(i => i).ToList();
        var candidatePopulation = new Population();
        foreach (var candidate in allCandidates)
            candidatePopulation.Add(candidate);
        var chosenIndividual = bestSelection.Process(candidatePopulation)[0];

        return (TTransformer)(ITransformer)new LogicGpTransformer(
            chosenIndividual, data);
    }

    public SchemaShape GetOutputSchema(SchemaShape inputSchema)
    {
        throw new NotImplementedException();
    }
}