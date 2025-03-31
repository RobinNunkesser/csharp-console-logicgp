using Italbytz.Adapters.Algorithms.AI.Util;
using logicGP.Tests.Data;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class SolarflareTests : RealTests
{
    private readonly IDataView _data;

    public SolarflareTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "solarflare_1.csv");
        _data = mlContext.Data.LoadFromTextFile<SolarflareModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        ThreadSafeRandomNetCore.Seed = 42;
        var accuracy = TestFlRw(_data, "flares", 10);
        Assert.IsTrue(accuracy > 0.2238);
        Assert.IsTrue(accuracy < 0.2239);
    }
}