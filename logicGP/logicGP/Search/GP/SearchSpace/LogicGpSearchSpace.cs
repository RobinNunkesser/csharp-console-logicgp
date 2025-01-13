using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpSearchSpace(IGeneticProgram gp, DataManager data)
    : ISearchSpace
{
    public int Classes { get; set; } = 2;

    public IGenotype GetRandomGenotype()
    {
        return new LogicGpGenotype(Classes, data);
    }

    public IIndividualList GetAStartingPopulation()
    {
        var result = new Population();
        foreach (var polynomial in data.Literals
                     .Select(
                         literal =>
                             new LogicGpMonomial<string>([literal], 2))
                     .Select(monomial =>
                         new LogicGpPolynomial<string>([monomial])))
            result.Add(new Individual(new LogicGpGenotype(polynomial, data),
                null));

        return result;
    }
}