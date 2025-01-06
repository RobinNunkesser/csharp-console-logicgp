using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;

public class GenerationStoppingCriterion(IGeneticProgram gp)
    : IStoppingCriterion
{
    public int Limit { get; set; }

    public bool IsMet()
    {
        return gp.Generation >= Limit;
    }
}