using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpGpasBinaryTrainer(
    LogicGpAlgorithm algorithm)
    : LogicGpTrainerBase<ITransformer>
{
    public override ITransformer Fit(IDataView input)
    {
        var k = 5; // Number of folds
        var mlContext = new MLContext();
        var cvResults = mlContext.Data.CrossValidationSplit(input, k);
        var candidates = new IIndividualList[k];
        var foldIndex = 0;
        DataFactory.Instance.Initialize(cvResults[0].TrainSet, "y");
        foreach (var fold in cvResults)
        {
            if (foldIndex > 0)
                foreach (var literal in DataFactory.Instance.Literals)
                    literal.GeneratePredictions(
                        fold.TrainSet.GetColumn<float>(literal.Label).ToList());
            var individuals = algorithm.Fit(fold.TrainSet);
            foreach (var literal in DataFactory.Instance.Literals)
                literal.GeneratePredictions(
                    fold.TestSet.GetColumn<float>(literal.Label).ToList());
            var fitness = new LogicGpPareto();
            foreach (var individual in individuals)
            {
                ((LogicGpGenotype)individual.Genotype)
                    .UpdatePredictionsRecursively();
                individual.Generation = 0;
                var fitnessValue = ((IFitnessFunction)fitness).Evaluate(
                    individual,
                    fold.TestSet, "y");
                var accuracy = 0.0;
                for (var i = 0; i < fitnessValue.Length - 1; i++)
                    accuracy += fitnessValue[i];
                individual.LatestKnownFitness =
                    new[] { accuracy, fitnessValue[^1] };
            }

            var selection = new ParetoFrontSelection();
            candidates[foldIndex++] = selection.Process(individuals);
        }

        var allCandidates = candidates.SelectMany(i => i).ToList();
        var groups = allCandidates
            .GroupBy(i => ((LogicGpGenotype)i.Genotype).LiteralSignature())
            .OrderByDescending(group => group.Count());
        var size = groups.FirstOrDefault()!.Count();
        var bestGroups = groups.Where(group => group.Count() == size);
        var bestGroup = bestGroups.First();
        var bestFitness = 0.0;
        foreach (var group in bestGroups)
        {
            var accumulatedFitness =
                group.Sum(element => element.LatestKnownFitness[0]);
            if (!(accumulatedFitness < bestFitness)) continue;
            bestFitness = accumulatedFitness;
            bestGroup = group;
        }

        var chosenIndividual = bestGroup.First();
        return new LogicGpTransformer(chosenIndividual);
    }
}