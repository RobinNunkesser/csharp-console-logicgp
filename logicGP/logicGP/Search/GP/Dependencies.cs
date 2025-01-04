using Italbytz.Adapters.Algorithms.AI.Search.GP.Initialization;
using Italbytz.Adapters.Algorithms.AI.Search.GP.PopulationManager;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using logicGP.Search.GP;
using Microsoft.Extensions.DependencyInjection;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public static class Dependencies
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IGeneticProgram, GeneticProgram>();
        services.AddSingleton<LogicGpAlgorithm>();
        services.AddSingleton<LogicGpFlrwBinaryTrainer>();
        services.AddSingleton<RandomInitialization>();
        services.AddSingleton<DefaultPopulationManager>();
        services.AddSingleton<LogicGpSearchSpace>();
        return services;
    }
}