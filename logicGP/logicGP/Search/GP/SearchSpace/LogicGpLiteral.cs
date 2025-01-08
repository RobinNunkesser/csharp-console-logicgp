using System.Text;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpLiteral<TCategory> : ILiteral<TCategory>
{
    private readonly List<TCategory> _orderedCategories;
    public readonly bool[] BitSet;

    public LogicGpLiteral(string label, HashSet<TCategory> categories, int set,
        List<TCategory> trainingData)
    {
        Label = label;
        Set = set;
        _orderedCategories = categories.OrderBy(c => c).ToList();
        BitSet = new bool[_orderedCategories.Count];
        for (var i = 0; i < _orderedCategories.Count; i++)
            BitSet[i] = (set & (1 << i)) != 0;
        GeneratePredictions(trainingData);
    }

    public int Set { get; }

    public string Label { get; set; }

    public bool[] Predictions { get; set; }

    public void GeneratePredictions(List<TCategory> data)
    {
        Predictions = new bool[data.Count];
        for (var i = 0; i < data.Count; i++)
        {
            var category = data[i];
            var index = _orderedCategories.IndexOf(category);
            Predictions[i] = BitSet[index];
        }
    }

    public int CompareTo(ILiteral<TCategory>? other)
    {
        return Compare(this, other);
    }

    private static int Compare(ILiteral<TCategory>? x, ILiteral<TCategory>? y)
    {
        if (x is null && y is null) return 0;
        if (x is not LogicGpLiteral<TCategory> literal1) return -1;
        if (y is not LogicGpLiteral<TCategory> literal2) return 1;
        if (x.Label != y.Label)
            return string.Compare(x.Label, y.Label, StringComparison.Ordinal);
        if (literal1.Set !=
            literal2.Set)
            return literal1.Set.CompareTo(
                literal2.Set);
        return 0;
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
        return HashCode.Combine(BitSet, _orderedCategories, Label);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"({Label} ∈ {{");
        for (var j = 0; j < _orderedCategories.Count; j++)
            if (BitSet[j])
                sb.Append(_orderedCategories[j] + ",");
        sb.Remove(sb.Length - 1, 1);
        sb.Append("})");
        return sb.ToString();
    }
}