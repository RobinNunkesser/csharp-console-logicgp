using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
using logicGP.Search.GP;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class GeneticProgram : IGeneticProgram
{
    public IDataView TrainingData { get; set; }
    public IIndividualList Population { get; set; }

    public IInitialization Initialization { get; set; }

    //public IFitnessFunction<TFitness> FitnessFunction { get; set; }
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
                /* PopulationManager.Select();
                 PopulationManager.Crossover();
                 PopulationManager.Mutate();*/
            }
        }

        return PopulationManager.Population;
    }
}