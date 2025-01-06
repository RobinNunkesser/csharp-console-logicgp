using System.Text;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpMonomial<TCategory> : IMonomial<TCategory>
{
    private readonly int _classes;

    public LogicGpMonomial(IEnumerable<ILiteral<TCategory>> literals,
        int classes)
    {
        Literals = literals.ToList();
        _classes = classes;
        Weights = new float[_classes];
        Weights[_classes - 1] = 1;
        UpdatePredictions();
    }

    public float[][] Predictions { get; set; }

    public IMonomial<TCategory> Clone()
    {
        return new LogicGpMonomial<TCategory>(Literals, _classes)
        {
            Weights = Weights
        };
    }

    public List<ILiteral<TCategory>> Literals { get; set; }
    public float[] Weights { get; set; }

    private void UpdatePredictions()
    {
        var literalPredictions = Literals[0].Predictions;
        if (Literals.Count > 1)
            literalPredictions = Literals.Aggregate(literalPredictions,
                (current, literal) =>
                    current.Zip(literal.Predictions, (a, b) => a && b)
                        .ToArray());

        Predictions = new float[Literals[0].Predictions.Length][];
        for (var i = 0; i < Predictions.Length; i++)
            if (literalPredictions[i])
                Predictions[i] = Weights;
            else
                Predictions[i] = new float[_classes];
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("(");
        sb.Append(string.Join(", ", Weights));
        sb.Append(")");
        sb.Append(string.Join("", Literals));
        return sb.ToString();
    }
}