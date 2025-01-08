using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpGenotype : IGenotype
{
    private readonly IPolynomial<float> _polynomial;
    private float[]? _predictedClasses;

    public LogicGpGenotype(int classes)
    {
        var literal = DataFactory.Instance.GetRandomLiteral();
        var monomial = new LogicGpMonomial<float>([literal], classes);
        _polynomial = new LogicGpPolynomial<float>([monomial]);
    }

    public LogicGpGenotype(IPolynomial<float> polynomial)
    {
        _polynomial = polynomial;
    }

    public float[] PredictedClasses
    {
        get
        {
            if (_predictedClasses == null) UpdatePredictedClasses();
            return _predictedClasses;
        }
        set => _predictedClasses = value;
    }


    public float[][] Predictions => _polynomial.Predictions;

    public double[]? LatestKnownFitness { get; set; }

    public int Size => _polynomial.Size;

    public void UpdatePredictions()
    {
        LatestKnownFitness = null;
        _polynomial.UpdatePredictions();
        UpdatePredictedClasses();
    }

    public object Clone()
    {
        return new LogicGpGenotype((IPolynomial<float>)_polynomial.Clone());
    }

    private void UpdatePredictedClasses()
    {
        _predictedClasses = new float[Predictions.Length];
        for (var i = 0; i < Predictions.Length; i++)
        {
            var maxIndex = Array.IndexOf(Predictions[i], Predictions[i].Max());
            _predictedClasses[i] = DataFactory.Instance.Labels[maxIndex];
        }
    }

    public override string ToString()
    {
        return _polynomial.ToString() ?? string.Empty;
    }

    public IMonomial<float> GetRandomMonomial()
    {
        return _polynomial.GetRandomMonomial();
    }

    public void InsertMonomial(IMonomial<float> monomial)
    {
        _polynomial.Monomials.Add(monomial);
        UpdatePredictions();
    }

    public void RandomizeAMonomialWeight()
    {
        var monomial = GetRandomMonomial();
        monomial.RandomizeWeights();
        UpdatePredictions();
    }

    public void DeleteRandomLiteral()
    {
        var monomial = GetRandomMonomial();
        monomial.Literals.RemoveAt(new Random().Next(monomial.Literals.Count));
        if (monomial.Literals.Count == 0)
            _polynomial.Monomials.Remove(monomial);
        else
            monomial.UpdatePredictions();
        UpdatePredictions();
    }

    public bool IsEmpty()
    {
        return _polynomial.Monomials.Count == 0;
    }

    public void DeleteRandomMonomial()
    {
        _polynomial.Monomials.RemoveAt(
            new Random().Next(_polynomial.Monomials.Count));
        if (_polynomial.Monomials.Count > 0)
            UpdatePredictions();
    }

    public void InsertRandomLiteral()
    {
        var monomial = GetRandomMonomial();
        monomial.Literals.Add(DataFactory.Instance.GetRandomLiteral());
        monomial.UpdatePredictions();
        UpdatePredictions();
    }

    public void InsertRandomMonomial()
    {
        _polynomial.Monomials.Add(new LogicGpMonomial<float>(
            new List<ILiteral<float>>
                { DataFactory.Instance.GetRandomLiteral() },
            _polynomial.Monomials[0].Weights.Length));
        UpdatePredictions();
    }

    public void ReplaceRandomLiteral()
    {
        var monomial = GetRandomMonomial();
        monomial.Literals[new Random().Next(monomial.Literals.Count)] =
            DataFactory.Instance.GetRandomLiteral();
        monomial.UpdatePredictions();
        UpdatePredictions();
    }

    public void UpdatePredictionsRecursively()
    {
        foreach (var monomial in _polynomial.Monomials)
            monomial.UpdatePredictions();
        _polynomial.UpdatePredictions();
        UpdatePredictedClasses();
    }

    public string LiteralSignature()
    {
        var literals = _polynomial.GetAllLiterals();
        literals.Sort();
        return string.Join(" ", literals);
    }

    public bool IsLiterallyEqual(LogicGpGenotype other)
    {
        var literals = _polynomial.GetAllLiterals();
        var otherLiterals = other._polynomial.GetAllLiterals();
        if (literals.Count != otherLiterals.Count) return false;

        return !literals.Except(otherLiterals).Any() &&
               !otherLiterals.Except(literals).Any();
    }
}