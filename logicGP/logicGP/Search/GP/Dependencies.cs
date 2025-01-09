using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;
using Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Selection;
using Italbytz.Adapters.Algorithms.AI.Search.GP.StoppingCriterion;
using logicGP.Search.GP;
using Microsoft.Extensions.DependencyInjection;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public static class Dependencies
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services
            .AddSingleton<IGeneticProgram,
                GeneticProgram>();
        services.AddSingleton<LogicGpAlgorithm>();
        services.AddSingleton<LogicGpGpasBinaryTrainer>();
        services.AddSingleton<RandomInitialization>();
        services.AddSingleton<CompleteInitialization>();
        services.AddSingleton<GenerationStoppingCriterion>();
        services.AddSingleton<UniformSelection>();
        services.AddSingleton<ParetoFrontSelection>();
        services.AddSingleton<IFitnessFunction, LogicGpPareto>();
        services.AddSingleton<DefaultPopulationManager>();
        services.AddSingleton<LogicGpSearchSpace>();
        return services;
    }
}