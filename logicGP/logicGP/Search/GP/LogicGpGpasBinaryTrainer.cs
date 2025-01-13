using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpGpasBinaryTrainer(
    LogicGpAlgorithm algorithm,
    DataFactory data)
    : LogicGpTrainerBase<ITransformer>
{
    protected override ITransformer ConcreteFit(IDataView input, string label)
    {
        // Split data into k folds
        const int k = 5; // Number of folds
        var mlContext = new MLContext();
        var cvResults = mlContext.Data.CrossValidationSplit(input);
        var candidates = new IIndividualList[k];
        var foldIndex = 0;
        data.Initialize(cvResults[0].TrainSet, label);
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
                LabelColumnName = label,
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

        return new LogicGpTransformer(chosenIndividual, data);
    }
}