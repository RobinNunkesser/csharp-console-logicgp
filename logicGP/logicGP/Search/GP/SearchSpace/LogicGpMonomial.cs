using System.Text;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpMonomial<TCategory> : IMonomial<TCategory>
{
    private readonly int _classes;

    public LogicGpMonomial(IEnumerable<ILiteral<TCategory>> literals,
        int classes, List<string>? outputValues, List<string> labels)
    {
        ArgumentNullException.ThrowIfNull(outputValues);
        ArgumentNullException.ThrowIfNull(labels);
        Labels = labels;
        OutputColumn = outputValues;
        Literals = literals.ToList();
        _classes = classes;
        Weights = new float[_classes];
        Weights[_classes - 1] = 1;
        UpdatePredictions();
    }

    public List<string> Labels { get; set; }

    public List<string>? OutputColumn { get; set; }

    public float[] CounterWeights { get; set; }

    public float[][] Predictions { get; set; }

    public int Size => Literals.Count;

    public void RandomizeWeights(bool restricted)
    {
        var random = new Random();
        if (restricted)
        {
            var index = random.Next(0, Weights.Length);
            for (var i = 0; i < Weights.Length; i++)
                Weights[i] = i == index ? 1 : 0;
        }
        else
        {
            for (var i = 0; i < Weights.Length; i++)
                Weights[i] = (float)random.NextDouble();
        }

        UpdatePredictions();
    }

    public object Clone()
    {
        return new LogicGpMonomial<TCategory>(Literals, _classes, OutputColumn,
            Labels)
        {
            Weights = new float[_classes].Select((_, i) => Weights[i]).ToArray()
        };
    }

    public List<ILiteral<TCategory>> Literals { get; set; }
    public float[] Weights { get; set; }

    public void UpdatePredictions()
    {
        var literalPredictions = Literals[0].Predictions;
        if (Literals.Count > 1)
            literalPredictions = Literals.Aggregate(literalPredictions,
                (current, literal) =>
                    current.Zip(literal.Predictions, (a, b) => a && b)
                        .ToArray());

        Predictions = new float[Literals[0].Predictions.Length][];

        // TODO: This is a hack for quick and dirty weight computation

        if (Predictions.Length == OutputColumn.Count)
            ComputeWeights(literalPredictions);


        for (var i = 0; i < Predictions.Length; i++)
            if (literalPredictions[i])
                Predictions[i] = Weights;
            else
                Predictions[i] = CounterWeights;
    }

    private void ComputeWeights(bool[] literalPredictions)
    {
        var count = new int[_classes];
        var counterCount = new int[_classes];
        for (var i = 0; i < Predictions.Length; i++)
        {
            var index = Labels.IndexOf(OutputColumn[i]);
            if (index == -1 || index >= _classes)
                throw new InvalidOperationException();
            if (literalPredictions[i])
                count[index]++;
            // else
            //counterCount[index]++;
        }

        var weights = count.Select(c => (float)c).ToArray();
        var counterWeights = counterCount.Select(c => (float)c).ToArray();
        /*var sum = weights.Sum();
        if (sum == 0)
            sum = 1;
        for (var j = 0; j < weights.Length; j++)
            weights[j] /= sum;*/

        /*sum = counterWeights.Sum();
        if (sum == 0)
            sum = 1;
        for (var j = 0; j < counterWeights.Length; j++)
            counterWeights[j] /= sum;*/

        Weights = weights;
        CounterWeights = counterWeights;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append('(');
        sb.Append(string.Join(",", Weights));
        sb.Append(')');
        sb.Append(string.Join("", Literals));
        return sb.ToString();
    }
}