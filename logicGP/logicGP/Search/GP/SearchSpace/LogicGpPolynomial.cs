namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpPolynomial<TCategory> : IPolynomial<TCategory>
{
    public LogicGpPolynomial(IEnumerable<IMonomial<TCategory>> monomials)
    {
        Monomials = [..monomials];
        UpdatePredictions();
    }

    public float[][] Predictions { get; set; }

    public IMonomial<TCategory> GetRandomMonomial()
    {
        var random = new Random();
        return Monomials[random.Next(Monomials.Count)];
    }

    public object Clone()
    {
        var monomials =
            Monomials.Select(
                monomial => (IMonomial<TCategory>)monomial.Clone());
        return new LogicGpPolynomial<TCategory>(
            monomials);
    }

    public List<IMonomial<TCategory>> Monomials { get; set; }

    public void UpdatePredictions()
    {
        if (Monomials.Count == 0) return;
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