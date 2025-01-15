using System.Collections;
using Italbytz.Adapters.Algorithms.AI.Search.Framework;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Crossover;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Mutation;
using Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
using Italbytz.Ports.Algorithms.AI.Search;
using logicGP.Search.GP;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpAlgorithm(
    IGeneticProgram gp,
    RandomInitialization randomInitialization,
    CompleteInitialization completeInitialization,
    DefaultPopulationManager populationManager,
    LogicGpSearchSpace searchSpace,
    GenerationStoppingCriterion generationStoppingCriterion,
    UniformSelection selection,
    ParetoFrontSelection paretoFrontSelection,
    IFitnessFunction fitnessFunction,
    DataManager data)
{
    public enum PredictionStrategy
    {
        Max,
        SoftmaxProbability
    }

    public enum WeightMutation
    {
        None,
        Restricted,
        Unrestricted
    }

    public bool UseFullInitialization { get; set; } = false;

    public WeightMutation WeightMutationToUse { get; set; } =
        WeightMutation.None;

    public IMetrics? Validate(IDataView validationData,
        IIndividualList individuals,
        string labelColumnName = DefaultColumnNames.Label)
    {
        var labelColumn = validationData.GetColumnAsString(labelColumnName)
            .ToList();
        var labelDistribution = new float[data.Labels.Count];
        foreach (var label in labelColumn)
            labelDistribution[data.Labels.IndexOf(label)]++;
        foreach (var literal in data.Literals)
            literal.GeneratePredictions(
                validationData.GetColumnAsString(literal.Label).ToList());
        foreach (var individual in individuals)
        {
            ((LogicGpGenotype)individual.Genotype)
                .UpdatePredictionsRecursively();
            individual.Generation = 0;
            var fitnessValue = fitnessFunction.Evaluate(
                individual,
                validationData);

            var macroAccuracy = 0.0;
            for (var i = 0; i < fitnessValue.Length - 1; i++)
                macroAccuracy += fitnessValue[i] / labelDistribution[i];

            macroAccuracy /= fitnessValue.Length - 1;

            //var accuracy = 0.0;
            //for (var i = 0; i < fitnessValue.Length - 1; i++)
            //    accuracy += fitnessValue[i];
            individual.LatestKnownFitness =
                [macroAccuracy, fitnessValue[^1]];
        }

        return new Metrics();
    }

    public IMetrics? Test(IDataView testData)
    {
        return new Metrics();
    }

    public IIndividualList Train(IDataView trainData,
        bool firstTraining = true,
        string labelColumnName = DefaultColumnNames.Label
    )
    {
        if (firstTraining)
            PrepareForFirstTraining(trainData, labelColumnName);
        else
            PrepareForRetraining(trainData, labelColumnName);

        randomInitialization.Size = 2;
        //generationStoppingCriterion.Limit = 10000;
        generationStoppingCriterion.Limit = 10000;
        selection.Size = 6;
        gp.SelectionForOperator = selection;
        gp.SelectionForSurvival = paretoFrontSelection;
        gp.PopulationManager = populationManager;
        gp.TrainingData = trainData;
        gp.Initialization = UseFullInitialization
            ? completeInitialization
            : randomInitialization;
        gp.Crossovers = [new LogicGpCrossover()];
        gp.Mutations =
        [
            new DeleteLiteral(), new InsertLiteral(),
            new InsertMonomial(), new ReplaceLiteral(), new DeleteMonomial()
        ];

        IMutation? weightMutation = WeightMutationToUse switch
        {
            WeightMutation.None => null,
            WeightMutation.Restricted => new ChangeWeightsRestricted(),
            WeightMutation.Unrestricted => new ChangeWeightsUnrestricted(),
            _ => null
        };

        if (weightMutation != null)
            ((IList)gp.Mutations).Add(weightMutation);
        fitnessFunction.LabelColumnName = data.Label;
        ((LogicGpPareto)fitnessFunction).Labels = data.Labels;
        gp.FitnessFunction = fitnessFunction;
        searchSpace.OutputColumn =
            trainData.GetColumnAsString(labelColumnName).ToList();
        gp.SearchSpace = searchSpace;
        gp.StoppingCriteria = [generationStoppingCriterion];
        return gp.Run();
    }

    private void PrepareForFirstTraining(IDataView trainData,
        string labelColumnName)
    {
        data.Initialize(trainData, labelColumnName);
    }


    private void PrepareForRetraining(IDataView trainData,
        string labelColumnName)
    {
        foreach (var literal in data.Literals)
            literal.GeneratePredictions(
                trainData.GetColumnAsString(literal.Label)
                    .ToList());
    }
}