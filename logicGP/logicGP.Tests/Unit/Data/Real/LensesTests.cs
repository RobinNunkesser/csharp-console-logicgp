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
        TestFlRw(_data, "class");
    }
}