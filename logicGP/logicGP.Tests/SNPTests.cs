using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

[TestClass]
public sealed class SNPTests
{
    private readonly IDataView _data;

    public SNPTests()
    {
        var mlContext = new MLContext();
        _data = mlContext.Data.LoadFromTextFile<SNPModelInput>(
            "/Users/nunkesser/repos/work/articles/logicgp/data/snpaccuracy/laumain_s500_o15_p0225_n44/SNPglm_2.csv",
            ',', true);
    }

    [TestInitialize]
    public void TestInitialize()
    {
    }

    [TestMethod]
    public void TestLiteralSignatureEquality()
    {
        var data = new DataManager();
        data.Initialize(_data, "y");
        var literal1 = data.GetRandomLiteral();
        var literal2 = data.GetRandomLiteral();
        var literal3 = data.GetRandomLiteral();
        var literal4 = data.GetRandomLiteral();
        var literal5 = data.GetRandomLiteral();
        var monomial1 = new LogicGpMonomial<float>([literal1, literal2], 2);
        var monomial2 =
            new LogicGpMonomial<float>([literal3, literal4, literal5], 2);
        var monomial3 = new LogicGpMonomial<float>([literal1, literal3], 2);
        var monomial4 =
            new LogicGpMonomial<float>([literal2, literal4, literal5], 2);
        var monomial5 =
            new LogicGpMonomial<float>(
                [literal1, literal2, literal3, literal4, literal5], 2);
        var polynomial1 = new LogicGpPolynomial<float>([monomial1, monomial2]);
        var polynomial2 = new LogicGpPolynomial<float>([monomial3, monomial4]);
        var polynomial3 = new LogicGpPolynomial<float>([monomial5]);
        var polynomial4 = new LogicGpPolynomial<float>([monomial1, monomial3]);
        var genotype1 = new LogicGpGenotype(polynomial1, data);
        var genotype2 = new LogicGpGenotype(polynomial2, data);
        var genotype3 = new LogicGpGenotype(polynomial3, data);
        var genotype4 = new LogicGpGenotype(polynomial4, data);
        var signature1 = genotype1.LiteralSignature();
        var signature2 = genotype2.LiteralSignature();
        var signature3 = genotype3.LiteralSignature();
        var signature4 = genotype4.LiteralSignature();
        Assert.AreEqual(signature1, signature2);
        Assert.AreEqual(signature2, signature3);
        Assert.AreEqual(signature3, signature1);
        Assert.AreNotEqual(signature4, signature1);
        Assert.AreNotEqual(signature4, signature2);
        Assert.AreNotEqual(signature4, signature3);
    }

    [TestMethod]
    public void TestLiteralEquality()
    {
        var data = new DataManager();
        data.Initialize(_data, "y");
        var literal1 = data.GetRandomLiteral();
        var literal2 = data.GetRandomLiteral();
        var literal3 = data.GetRandomLiteral();
        var literal4 = data.GetRandomLiteral();
        var literal5 = data.GetRandomLiteral();
        var monomial1 = new LogicGpMonomial<float>([literal1, literal2], 2);
        var monomial2 =
            new LogicGpMonomial<float>([literal3, literal4, literal5], 2);
        var monomial3 = new LogicGpMonomial<float>([literal1, literal3], 2);
        var monomial4 =
            new LogicGpMonomial<float>([literal2, literal4, literal5], 2);
        var monomial5 =
            new LogicGpMonomial<float>(
                [literal1, literal2, literal3, literal4, literal5], 2);
        var polynomial1 = new LogicGpPolynomial<float>([monomial1, monomial2]);
        var polynomial2 = new LogicGpPolynomial<float>([monomial3, monomial4]);
        var polynomial3 = new LogicGpPolynomial<float>([monomial5]);
        var polynomial4 = new LogicGpPolynomial<float>([monomial1, monomial3]);
        var genotype1 = new LogicGpGenotype(polynomial1, data);
        var genotype2 = new LogicGpGenotype(polynomial2, data);
        var genotype3 = new LogicGpGenotype(polynomial3, data);
        var genotype4 = new LogicGpGenotype(polynomial4, data);
        Assert.IsTrue(genotype1.IsLiterallyEqual(genotype2));
        Assert.IsTrue(genotype2.IsLiterallyEqual(genotype3));
        Assert.IsTrue(genotype3.IsLiterallyEqual(genotype1));
        Assert.IsFalse(genotype4.IsLiterallyEqual(genotype1));
        Assert.IsFalse(genotype4.IsLiterallyEqual(genotype2));
        Assert.IsFalse(genotype4.IsLiterallyEqual(genotype3));
    }

    [TestMethod]
    public void TestGPAS()
    {
        var services = new ServiceCollection().AddServices();
        var serviceProvider = services.BuildServiceProvider();
        var trainer =
            serviceProvider.GetRequiredService<LogicGpGpasBinaryTrainer>();
        trainer.Label = "y";
        var mlModel = trainer.Fit(_data);
        Assert.IsNotNull(mlModel);
        var testResults = mlModel.Transform(_data);
        var trueValues = testResults.GetColumn<uint>("y").ToArray();
        var predictedValues = testResults.GetColumn<float[]>("Score")
            .Select(score => score[0] >= 0.5 ? 0 : 1).ToArray();
        var mcr = 0F;

        for (var i = 0; i < predictedValues.Length; i++)
            if (predictedValues[i] != trueValues[i])
                mcr++;

        mcr /= predictedValues.Length;
        var acc = 1.0 - mcr;
        Console.WriteLine($"{acc}");
    }
}