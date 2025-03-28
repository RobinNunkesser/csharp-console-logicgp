using logicGP.Tests.Data;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class RestaurantTests : RealTests
{
    private readonly IDataView _data;

    public RestaurantTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "restaurant.csv");
        _data = mlContext.Data.LoadFromTextFile<RestaurantModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        TestFlRw(_data, "will_wait");
    }
}