using System.Text;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpLiteral<TCategory> : ILiteral<TCategory>
{
    private readonly bool[] _bitSet;
    private readonly List<TCategory> _orderedCategories;

    public LogicGpLiteral(string label, HashSet<TCategory> categories, int set,
        List<TCategory> trainingData)
    {
        Label = label;
        _orderedCategories = categories.OrderBy(c => c).ToList();
        _bitSet = new bool[_orderedCategories.Count];
        for (var i = 0; i < _orderedCategories.Count; i++)
            _bitSet[i] = (set & (1 << i)) != 0;
        GeneratePredictions(trainingData);
    }

    public string Label { get; set; }

    public bool[] Predictions { get; set; }

    public void GeneratePredictions(List<TCategory> data)
    {
        Predictions = new bool[data.Count];
        for (var i = 0; i < data.Count; i++)
        {
            var category = data[i];
            var index = _orderedCategories.IndexOf(category);
            Predictions[i] = _bitSet[index];
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((LogicGpLiteral<TCategory>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_bitSet, _orderedCategories, Label);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"({Label} ∈ {{");
        for (var j = 0; j < _orderedCategories.Count; j++)
            if (_bitSet[j])
                sb.Append(_orderedCategories[j] + ",");
        sb.Remove(sb.Length - 1, 1);
        sb.Append("})");
        return sb.ToString();
    }
}