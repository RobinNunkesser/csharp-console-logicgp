using Italbytz.Adapters.Algorithms.AI.Util;
using logicGP.Tests.Data;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class LensesTests : RealTests
{
    private readonly IDataView _data;

    public LensesTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "lenses.csv");
        _data = mlContext.Data.LoadFromTextFile<LensesModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        ThreadSafeRandomNetCore.Seed = 42;
        var accuracy = TestFlRw(_data, "class", 10);
        Assert.IsTrue(accuracy > 0.8000);
        Assert.IsTrue(accuracy < 0.8001);
    }
}