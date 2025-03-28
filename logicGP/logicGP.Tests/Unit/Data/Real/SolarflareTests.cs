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
            "Data", "solarflare_1.csv");
        _data = mlContext.Data.LoadFromTextFile<SolarflareModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        TestFlRw(_data, "flares");
    }
}