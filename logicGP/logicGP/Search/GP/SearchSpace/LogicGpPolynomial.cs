using System.Text;
using Italbytz.Adapters.Algorithms.AI.Util;
using Italbytz.Ports.Algorithms.AI.Search.GP.SearchSpace;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

/// <summary>
///     A class representing a polynomial in the LogicGP algorithm.
///     It implements the IPolynomial interface and provides methods for generating
///     predictions and comparing polynomials.
/// </summary>
/// <typeparam name="TCategory">The type of the categories used in the polynomial.</typeparam>
/// <remarks>
///     The LogicGpPolynomial class is used to represent a polynomial in the LogicGP
///     algorithm.
///     It contains a list of monomials that represent the polynomial and provides
///     methods for generating predictions based on the monomials.
///     The class also provides methods for updating predictions and checking the size
///     of the polynomial.
/// </remarks>
/// <seealso cref="IPolynomial{TCategory}" />
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
        var random = ThreadSafeRandomNetCore.LocalRandom;
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
        var count = new int[_classes];
        for (var i = 0; i < Predictions.Length; i++)
        {
            Predictions[i] = Monomials
                .Select(monomial => monomial.Predictions[i].ToArray())
                .Aggregate((a, b) =>
                    a.Zip(b, (c, d) => c + d).ToArray());
            if (Predictions.Length == _outputValues.Count &&
                Predictions[i].Sum() == 0)
                count[_labels.IndexOf(_outputValues[i])]++;
            var sum = 0.0f;
            for (var j = 0; j < Predictions[i].Length; j++)
                //Predictions[i][j] += Weights[j];
                sum += Predictions[i][j];

            for (var j = 0; j < Predictions[i].Length; j++)
                Predictions[i][j] /= sum;
        }

        if (count.Sum() > 0)
            ComputeWeightsForCount(count);

        foreach (var pred in Predictions)
        {
            if (pred.Sum() != 0) continue;
            for (var j = 0; j < pred.Length; j++)
                pred[j] = Weights[j];
        }
    }

    public List<ILiteral<TCategory>> GetAllLiterals()
    {
        return Monomials.SelectMany(monomial => monomial.Literals).ToList();
    }

    private void ComputeWeightsForCount(int[] count)
    {
        var weights = count.Select(c => (float)c).ToArray();

        var sum = weights.Sum();
        if (sum == 0)
            sum = 1;
        for (var j = 0; j < weights.Length; j++)
            weights[j] /= sum;

        Weights = weights;
    }

    private void ComputeWeights()
    {
        var count = new int[_classes];
        foreach (var index in _outputValues.Select(value =>
                     _labels.IndexOf(value))) count[index]++;

        ComputeWeightsForCount(count);

        //Weights = [1.0f, 0.0f];
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