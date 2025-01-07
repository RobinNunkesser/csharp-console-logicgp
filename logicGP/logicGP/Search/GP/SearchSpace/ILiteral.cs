namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public interface ILiteral<TCategory>
{
    public bool[] Predictions { get; set; }
    public string Label { get; set; }
    public void GeneratePredictions(List<TCategory> data);
}