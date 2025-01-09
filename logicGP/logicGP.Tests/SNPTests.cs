﻿using Italbytz.Adapters.Algorithms.AI.Search.GP;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace logicGP.Tests;

//[TestClass]
public sealed class SNPTests
{
    private readonly IDataView _data;

    public SNPTests()
    {
        var mlContext = new MLContext();
        _data = mlContext.Data.LoadFromTextFile<ModelInput>(
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
        DataFactory.Instance.Initialize(_data, "y");
        var literal1 = DataFactory.Instance.GetRandomLiteral();
        var literal2 = DataFactory.Instance.GetRandomLiteral();
        var literal3 = DataFactory.Instance.GetRandomLiteral();
        var literal4 = DataFactory.Instance.GetRandomLiteral();
        var literal5 = DataFactory.Instance.GetRandomLiteral();
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
        var genotype1 = new LogicGpGenotype(polynomial1);
        var genotype2 = new LogicGpGenotype(polynomial2);
        var genotype3 = new LogicGpGenotype(polynomial3);
        var genotype4 = new LogicGpGenotype(polynomial4);
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
        DataFactory.Instance.Initialize(_data, "y");
        var literal1 = DataFactory.Instance.GetRandomLiteral();
        var literal2 = DataFactory.Instance.GetRandomLiteral();
        var literal3 = DataFactory.Instance.GetRandomLiteral();
        var literal4 = DataFactory.Instance.GetRandomLiteral();
        var literal5 = DataFactory.Instance.GetRandomLiteral();
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
        var genotype1 = new LogicGpGenotype(polynomial1);
        var genotype2 = new LogicGpGenotype(polynomial2);
        var genotype3 = new LogicGpGenotype(polynomial3);
        var genotype4 = new LogicGpGenotype(polynomial4);
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

public class ModelInput
{
    [LoadColumn(0)] [ColumnName(@"y")] public float Y { get; set; }

    [LoadColumn(1)] [ColumnName(@"SNP1")] public float SNP1 { get; set; }

    [LoadColumn(2)] [ColumnName(@"SNP2")] public float SNP2 { get; set; }

    [LoadColumn(3)] [ColumnName(@"SNP3")] public float SNP3 { get; set; }

    [LoadColumn(4)] [ColumnName(@"SNP4")] public float SNP4 { get; set; }

    [LoadColumn(5)] [ColumnName(@"SNP5")] public float SNP5 { get; set; }

    [LoadColumn(6)] [ColumnName(@"SNP6")] public float SNP6 { get; set; }

    [LoadColumn(7)] [ColumnName(@"SNP7")] public float SNP7 { get; set; }

    [LoadColumn(8)] [ColumnName(@"SNP8")] public float SNP8 { get; set; }

    [LoadColumn(9)] [ColumnName(@"SNP9")] public float SNP9 { get; set; }

    [LoadColumn(10)]
    [ColumnName(@"SNP10")]
    public float SNP10 { get; set; }

    [LoadColumn(11)]
    [ColumnName(@"SNP11")]
    public float SNP11 { get; set; }

    [LoadColumn(12)]
    [ColumnName(@"SNP12")]
    public float SNP12 { get; set; }

    [LoadColumn(13)]
    [ColumnName(@"SNP13")]
    public float SNP13 { get; set; }

    [LoadColumn(14)]
    [ColumnName(@"SNP14")]
    public float SNP14 { get; set; }

    [LoadColumn(15)]
    [ColumnName(@"SNP15")]
    public float SNP15 { get; set; }

    [LoadColumn(16)]
    [ColumnName(@"SNP16")]
    public float SNP16 { get; set; }

    [LoadColumn(17)]
    [ColumnName(@"SNP17")]
    public float SNP17 { get; set; }

    [LoadColumn(18)]
    [ColumnName(@"SNP18")]
    public float SNP18 { get; set; }

    [LoadColumn(19)]
    [ColumnName(@"SNP19")]
    public float SNP19 { get; set; }

    [LoadColumn(20)]
    [ColumnName(@"SNP20")]
    public float SNP20 { get; set; }

    [LoadColumn(21)]
    [ColumnName(@"SNP21")]
    public float SNP21 { get; set; }

    [LoadColumn(22)]
    [ColumnName(@"SNP22")]
    public float SNP22 { get; set; }

    [LoadColumn(23)]
    [ColumnName(@"SNP23")]
    public float SNP23 { get; set; }

    [LoadColumn(24)]
    [ColumnName(@"SNP24")]
    public float SNP24 { get; set; }

    [LoadColumn(25)]
    [ColumnName(@"SNP25")]
    public float SNP25 { get; set; }

    [LoadColumn(26)]
    [ColumnName(@"SNP26")]
    public float SNP26 { get; set; }

    [LoadColumn(27)]
    [ColumnName(@"SNP27")]
    public float SNP27 { get; set; }

    [LoadColumn(28)]
    [ColumnName(@"SNP28")]
    public float SNP28 { get; set; }

    [LoadColumn(29)]
    [ColumnName(@"SNP29")]
    public float SNP29 { get; set; }

    [LoadColumn(30)]
    [ColumnName(@"SNP30")]
    public float SNP30 { get; set; }

    [LoadColumn(31)]
    [ColumnName(@"SNP31")]
    public float SNP31 { get; set; }

    [LoadColumn(32)]
    [ColumnName(@"SNP32")]
    public float SNP32 { get; set; }

    [LoadColumn(33)]
    [ColumnName(@"SNP33")]
    public float SNP33 { get; set; }

    [LoadColumn(34)]
    [ColumnName(@"SNP34")]
    public float SNP34 { get; set; }

    [LoadColumn(35)]
    [ColumnName(@"SNP35")]
    public float SNP35 { get; set; }

    [LoadColumn(36)]
    [ColumnName(@"SNP36")]
    public float SNP36 { get; set; }

    [LoadColumn(37)]
    [ColumnName(@"SNP37")]
    public float SNP37 { get; set; }

    [LoadColumn(38)]
    [ColumnName(@"SNP38")]
    public float SNP38 { get; set; }

    [LoadColumn(39)]
    [ColumnName(@"SNP39")]
    public float SNP39 { get; set; }

    [LoadColumn(40)]
    [ColumnName(@"SNP40")]
    public float SNP40 { get; set; }

    [LoadColumn(41)]
    [ColumnName(@"SNP41")]
    public float SNP41 { get; set; }

    [LoadColumn(42)]
    [ColumnName(@"SNP42")]
    public float SNP42 { get; set; }

    [LoadColumn(43)]
    [ColumnName(@"SNP43")]
    public float SNP43 { get; set; }

    [LoadColumn(44)]
    [ColumnName(@"SNP44")]
    public float SNP44 { get; set; }

    [LoadColumn(45)]
    [ColumnName(@"SNP45")]
    public float SNP45 { get; set; }

    [LoadColumn(46)]
    [ColumnName(@"SNP46")]
    public float SNP46 { get; set; }

    [LoadColumn(47)]
    [ColumnName(@"SNP47")]
    public float SNP47 { get; set; }

    [LoadColumn(48)]
    [ColumnName(@"SNP48")]
    public float SNP48 { get; set; }

    [LoadColumn(49)]
    [ColumnName(@"SNP49")]
    public float SNP49 { get; set; }

    [LoadColumn(50)]
    [ColumnName(@"SNP50")]
    public float SNP50 { get; set; }
}