using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class CarEvaluationTests
{
    private readonly IDataView _data;

    public CarEvaluationTests()
    {
        var mlContext = new MLContext();
        _data = mlContext.Data.LoadFromTextFile<CarEvaluationModelInput>(
            "/Users/nunkesser/repos/work/articles/logicgp/data/ucimlrepo/car_evaluation.csv",
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider.GetRequiredService<LogicGpFlrwMulticlassTrainer>();
        trainer.Label = "class";
        var mlModel = trainer.Fit(_data);
        Assert.IsNotNull(mlModel);
    }
}