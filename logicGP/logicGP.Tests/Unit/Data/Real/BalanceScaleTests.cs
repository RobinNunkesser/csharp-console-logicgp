using Italbytz.Adapters.Algorithms.AI.Util;
using logicGP.Tests.Data;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class BalanceScaleTests : RealTests
{
    private readonly IDataView _data;

    public BalanceScaleTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "balancescale.csv");
        _data = mlContext.Data.LoadFromTextFile<BalanceScaleModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        ThreadSafeRandomNetCore.Seed = 42;
        TestFlRw(_data, "class");
    }
}