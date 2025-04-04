using Italbytz.Adapters.Algorithms.AI.Util;
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
            "Data/Real", "national_poll_on_healthy_aging_npha.csv");
        _data = mlContext.Data.LoadFromTextFile<NationalPollModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        ThreadSafeRandomNetCore.Seed = 42;
        var accuracy = TestFlRw(_data, "Number_of_Doctors_Visited", 10);
        Assert.IsTrue(accuracy > 0.3814);
        Assert.IsTrue(accuracy < 0.3815);
    }
}