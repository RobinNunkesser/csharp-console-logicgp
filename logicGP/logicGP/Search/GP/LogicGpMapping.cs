namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpMapping
{
    public static Action<TSrc, TDst> GetMapping<TSrc, TDst>()
        where TSrc : class, new() where TDst : class, new()
    {
        return Map;
    }

    private static void Map<TSrc, TDst>(TSrc arg1, TDst arg2)
        where TSrc : class, new() where TDst : class, new()
    {
        Console.WriteLine(arg1.ToString());
        throw new NotImplementedException();
    }
}