using logicGP.Tests.Data;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class NationalPollTests : RealTests
{
    private readonly IDataView _data;

    public NationalPollTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data", "national_poll_on_healthy_aging_npha.csv");
        _data = mlContext.Data.LoadFromTextFile<NationalPollModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        TestFlRw(_data, "class");
    }
}