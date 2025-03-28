using logicGP.Tests.Data;
using Microsoft.ML;

namespace logicGP.Tests;

[TestClass]
public class CarEvaluationTests : RealTests
{
    private readonly IDataView _data;

    public CarEvaluationTests()
    {
        var mlContext = new MLContext();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Data/Real", "car_evaluation_strings.csv");
        _data = mlContext.Data.LoadFromTextFile<CarEvaluationModelInput>(
            path,
            ',', true);
    }

    [TestMethod]
    public void TestFlRw()
    {
        TestFlRw(_data, "class");
    }
}