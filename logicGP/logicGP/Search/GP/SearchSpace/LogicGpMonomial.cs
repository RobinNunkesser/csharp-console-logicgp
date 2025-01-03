namespace Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;

public class LogicGpMonomial<TCategory> : IMonomial<TCategory>
{
    public LogicGpMonomial(ILiteral<TCategory> literal)
    {
        Literals = [literal];
        Weights = new float[] { 0, 1 };
    }

    public List<ILiteral<TCategory>> Literals { get; set; }
    public float[] Weights { get; set; }
}