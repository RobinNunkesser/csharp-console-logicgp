using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpGenotype : IGenotype
{
    private readonly IPolynomial<float> _polynomial;

    public LogicGpGenotype(int classes)
    {
        var literal = LiteralRepository.Instance.GetRandomLiteral();
        var monomial = new LogicGpMonomial<float>([literal], classes);
        _polynomial = new LogicGpPolynomial<float>([monomial]);
    }

    private LogicGpGenotype(IPolynomial<float> polynomial)
    {
        _polynomial = polynomial;
    }


    public float[][] Predictions => _polynomial.Predictions;

    public void UpdatePredictions()
    {
        _polynomial.UpdatePredictions();
    }

    public object Clone()
    {
        return new LogicGpGenotype((IPolynomial<float>)_polynomial.Clone());
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
        monomial.Literals.Add(LiteralRepository.Instance.GetRandomLiteral());
        monomial.UpdatePredictions();
        UpdatePredictions();
    }

    public void InsertRandomMonomial()
    {
        _polynomial.Monomials.Add(new LogicGpMonomial<float>(
            new List<ILiteral<float>>
                { LiteralRepository.Instance.GetRandomLiteral() },
            _polynomial.Monomials[0].Predictions.Length));
        UpdatePredictions();
    }

    public void ReplaceRandomLiteral()
    {
        var monomial = GetRandomMonomial();
        monomial.Literals[new Random().Next(monomial.Literals.Count)] =
            LiteralRepository.Instance.GetRandomLiteral();
        monomial.UpdatePredictions();
        UpdatePredictions();
    }
}