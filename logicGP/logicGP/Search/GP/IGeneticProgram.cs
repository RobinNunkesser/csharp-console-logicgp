using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;

namespace logicGP.Search.GP;

public interface IGeneticProgram
{
    public void InitPopulation();
    public IIndividual Run();

}