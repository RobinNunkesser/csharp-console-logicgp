using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpSearchSpace(IGeneticProgram gp) : ISearchSpace
{
    public int Classes { get; set; } = 2;

    public IGenotype GetRandomGenotype()
    {
        return new LogicGpGenotype(Classes);
    }

    public IIndividualList GetAStartingPopulation()
    {
        var literals = DataFactory.Instance.Literals;
        var result = new Population();
        foreach (var polynomial in literals
                     .Select(
                         literal => new LogicGpMonomial<float>([literal], 2))
                     .Select(monomial =>
                         new LogicGpPolynomial<float>([monomial])))
            result.Add(new Individual(new LogicGpGenotype(polynomial), null));

        return result;
    }
}