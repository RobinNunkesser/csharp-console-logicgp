using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using logicGP.Search.GP;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpSearchSpace(IGeneticProgram gp) : ISearchSpace
{
    private bool _initialized;
    public int Classes { get; set; } = 2;

    public IGenotype GetRandomGenotype()
    {
        if (_initialized) return new LogicGpGenotype(Classes);
        LiteralRepository.Instance.Initialize(gp.TrainingData);
        _initialized = true;

        return new LogicGpGenotype(Classes);
    }
}