using logicGP.Tests.Data.Simulated;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class MyCustomMapper
{
    public static Action<TSrc, TDst> GetMapping<TSrc, TDst>()
        where TSrc : class, new() where TDst : class, new()
    {
        return Fit;
    }

    private static void Fit<TSrc, TDst>(TSrc input, TDst output)
        where TSrc : class, new() where TDst : class, new()
    {
        if (output is not BinaryClassificationSchema schema) return;
        schema.Score = 1.0f;
        schema.Probability = 1.0f;
        schema.PredictedLabel = 42.0f;
    }
}