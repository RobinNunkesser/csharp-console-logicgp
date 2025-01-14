using System.Text;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpPolynomial<TCategory> : IPolynomial<TCategory>
{
    private readonly int _classes;
    private readonly List<string> _labels;
    private readonly List<string>? _outputValues;

    public LogicGpPolynomial(IEnumerable<IMonomial<TCategory>> monomials,
        int classes, List<string>? outputValues, List<string> labels)
    {
        _classes = classes;
        _outputValues = outputValues;
        _labels = labels;
        Monomials = [..monomials];
        Weights = new float[Monomials[0].Literals[0].Predictions.Length];
        ComputeWeights();
        UpdatePredictions();
    }

    public float[] Weights { get; set; }

    public float[][] Predictions { get; set; }

    public int Size
    {
        get { return Monomials.Sum(monomial => monomial.Size); }
    }

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
            monomials, _classes, _outputValues, _labels);
    }

    public List<IMonomial<TCategory>> Monomials { get; set; }

    public void UpdatePredictions()
    {
        if (Monomials.Count == 0) return;
        Predictions = new float[Monomials[0].Literals[0].Predictions.Length][];
        for (var i = 0; i < Predictions.Length; i++)
        {
            Predictions[i] = Monomials
                .Select(monomial => monomial.Predictions[i].ToArray())
                .Aggregate((a, b) =>
                    a.Zip(b, (c, d) => c + d).ToArray());
            var sum = 0.0f;
            for (var j = 0; j < Predictions[i].Length; j++)
            {
                Predictions[i][j] += Weights[j];
                sum += Predictions[i][j];
            }

            for (var j = 0; j < Predictions[i].Length; j++)
                Predictions[i][j] /= sum;
        }
    }

    public List<ILiteral<TCategory>> GetAllLiterals()
    {
        return Monomials.SelectMany(monomial => monomial.Literals).ToList();
    }

    private void ComputeWeights()
    {
        var count = new int[_classes];
        foreach (var index in _outputValues.Select(value =>
                     _labels.IndexOf(value))) count[index]++;

        var weights = count.Select(c => (float)c).ToArray();

        var sum = weights.Sum();
        if (sum == 0)
            sum = 1;
        for (var j = 0; j < weights.Length; j++)
            weights[j] /= sum;

        Weights = weights;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append('(');
        sb.Append(string.Join(", ", Weights));
        sb.Append(") + ");
        sb.Append(string.Join(" + ", Monomials));
        return sb.ToString();
    }
}