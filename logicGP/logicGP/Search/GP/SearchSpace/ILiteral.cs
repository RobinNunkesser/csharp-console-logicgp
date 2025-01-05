namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public interface ILiteral<TCategory>
{
    public bool[] Predictions { get; set; }
}