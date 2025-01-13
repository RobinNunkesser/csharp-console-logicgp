using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpSearchSpace(IGeneticProgram gp, DataManager data)
    : ISearchSpace
{
    public IGenotype GetRandomGenotype()
    {
        var classes = data.Labels.Distinct().Count();
        return new LogicGpGenotype(classes, data);
    }

    public IIndividualList GetAStartingPopulation()
    {
        var result = new Population();
        var classes = data.Labels.Distinct().Count();
        foreach (var polynomial in data.Literals
                     .Select(
                         literal =>
                             new LogicGpMonomial<string>([literal], classes))
                     .Select(monomial =>
                         new LogicGpPolynomial<string>([monomial])))
            result.Add(new Individual(new LogicGpGenotype(polynomial, data),
                null));

        return result;
    }
}