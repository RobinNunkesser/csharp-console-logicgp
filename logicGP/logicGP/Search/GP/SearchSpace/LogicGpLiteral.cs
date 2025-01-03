using System.Text;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpLiteral : ILiteral
{
    private readonly bool[] _bitSet;

    private readonly string _label;
    private readonly List<float> _orderedCategories;

    public LogicGpLiteral(string label, HashSet<float> categories, int set,
        List<float> trainingData)
    {
        _label = label;
        _orderedCategories = categories.OrderBy(c => c).ToList();
        _bitSet = new bool[_orderedCategories.Count];
        for (var i = 0; i < _orderedCategories.Count; i++)
            _bitSet[i] = (set & (1 << i)) != 0;
        GeneratePredictions(trainingData);
    }

    public bool[] Predictions { get; set; }

    private void GeneratePredictions(List<float> trainingData)
    {
        Predictions = new bool[trainingData.Count];
        for (var i = 0; i < trainingData.Count; i++)
        {
            var category = trainingData[i];
            var index = _orderedCategories.IndexOf(category);
            Predictions[i] = _bitSet[index];
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"({_label} ∈ {{");
        for (var j = 0; j < _orderedCategories.Count; j++)
            if (_bitSet[j])
                sb.Append(_orderedCategories[j] + ",");
        sb.Remove(sb.Length - 1, 1);
        sb.Append("})");
        return sb.ToString();
    }
}