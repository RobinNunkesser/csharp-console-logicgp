using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Crossover;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Mutation;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
using logicGP.Search.GP;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class GeneticProgram : IGeneticProgram
{
    public IFitnessFunction FitnessFunction { get; set; }
    public ISelection SelectionForOperator { get; set; }
    public ISelection SelectionForSurvival { get; set; }
    public IMutation[] Mutations { get; set; }
    public ICrossover[] Crossovers { get; set; }
    public IDataView TrainingData { get; set; }
    public IIndividualList Population => PopulationManager.Population;

    public IInitialization Initialization { get; set; }
    public IPopulationManager PopulationManager { get; set; }
    public ISearchSpace SearchSpace { get; set; }
    public IStoppingCriterion[] StoppingCriteria { get; set; }
    public int Generation { get; set; }

    public void InitPopulation()
    {
        PopulationManager.InitPopulation(Initialization);
    }

    public IIndividualList Run()
    {
        InitPopulation();
        var stop = false;
        while (!stop)
        {
            /*foreach (var individual in Population)
            {
                var fitness =
                    PopulationManager.FitnessFunction.Evaluate(individual,
                        TrainingData);
                individual.Fitness = fitness;
            }*/

            stop = StoppingCriteria.Any(sc => sc.IsMet());
            if (!stop)
            {
                foreach (var crossover in Crossovers)
                {
                    SelectionForOperator.Size = 2;
                    var selected = SelectionForOperator.Process(Population);
                    var children = crossover.Process(selected);
                }

                foreach (var mutation in Mutations)
                {
                    SelectionForOperator.Size = 1;
                    var selected = SelectionForOperator.Process(Population);
                    var mutated = mutation.Process(selected);
                }

                UpdatePopulationFitness();

                var newGeneration = SelectionForSurvival.Process(Population);
                PopulationManager.Population = newGeneration;
                Generation++;
            }
        }

        return PopulationManager.Population;
    }

    private void UpdatePopulationFitness()
    {
        foreach (var individual in Population)
        {
            if (individual.LatestKnownFitness != null) continue;
            var fitness =
                FitnessFunction.Evaluate(individual.Genotype,
                    TrainingData);
            individual.LatestKnownFitness = fitness;
        }
    }
}