namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpPolynomial<TCategory> : IPolynomial<TCategory>
{
    public LogicGpPolynomial(IEnumerable<LogicGpMonomial<TCategory>> monomials)
    {
        Monomials = [..monomials];
        UpdatePredictions();
    }

    public float[][] Predictions { get; set; }

    public IPolynomial<TCategory> Clone()
    {
        var monomials =
            (IEnumerable<LogicGpMonomial<TCategory>>)Monomials.Select(
                monomial => monomial.Clone());
        return new LogicGpPolynomial<TCategory>(monomials);
    }

    public List<IMonomial<TCategory>> Monomials { get; set; }

    private void UpdatePredictions()
    {
        Predictions = new float[Monomials[0].Literals[0].Predictions.Length][];
        for (var i = 0; i < Predictions.Length; i++)
            Predictions[i] = Monomials
                .Select(monomial => monomial.Predictions[i]).Aggregate((a, b) =>
                    a.Zip(b, (c, d) => c + d).ToArray());
    }

    public override string ToString()
    {
        return string.Join(" + ", Monomials);
    }
}