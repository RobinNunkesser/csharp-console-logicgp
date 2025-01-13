using System.Collections;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Crossover;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Mutation;
using Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
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
    public enum WeightMutation
    {
        None,
        Restricted,
        Unrestricted
    }

    public bool UseFullInitialization { get; set; } = false;

    public WeightMutation WeightMutationToUse { get; set; } =
        WeightMutation.None;

    public IIndividualList Fit(IDataView input)
    {
        randomInitialization.Size = 2;
        //generationStoppingCriterion.Limit = 10000;
        generationStoppingCriterion.Limit = 1000;
        selection.Size = 6;
        gp.SelectionForOperator = selection;
        gp.SelectionForSurvival = paretoFrontSelection;
        gp.PopulationManager = populationManager;
        gp.TrainingData = input;
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
        gp.SearchSpace = searchSpace;
        gp.StoppingCriteria = [generationStoppingCriterion];
        return gp.Run();
    }
}