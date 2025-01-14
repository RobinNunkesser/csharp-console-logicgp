using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpSearchSpace(IGeneticProgram gp, DataManager data)
    : ISearchSpace
{
    public List<string>? OutputColumn { get; set; }

    public IGenotype GetRandomGenotype()
    {
        var classes = data.Labels.Distinct().Count();
        return new LogicGpGenotype(classes, data, OutputColumn, data.Labels);
    }

    public IIndividualList GetAStartingPopulation()
    {
        var result = new Population();
        var classes = data.Labels.Distinct().Count();

        foreach (var polynomial in data.Literals
                     .Select(
                         literal =>
                             new LogicGpMonomial<string>([literal], classes,
                                 OutputColumn, data.Labels))
                     .Select(monomial =>
                         new LogicGpPolynomial<string>([monomial], classes,
                             OutputColumn, data.Labels)))
        {
            var newIndividual = new Individual(
                new LogicGpGenotype(polynomial, data, OutputColumn,
                    data.Labels),
                null);
            ((LogicGpGenotype)newIndividual.Genotype)
                .UpdatePredictionsRecursively();
            result.Add(newIndividual);
        }

        return result;
    }
}