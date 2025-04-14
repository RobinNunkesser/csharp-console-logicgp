using System.Runtime.CompilerServices;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class ThreadSafeMLContext
{
    private static int? _seed;
    [ThreadStatic] private static MLContext? _tMLContext;

    /// <summary>
    ///     Gets or sets the seed for random number generation.
    /// </summary>
    public static int? Seed
    {
        get => _seed;
        set
        {
            _seed = value;
            _tMLContext =
                null; // Reset to ensure next access creates a new Random with the seed
        }
    }

    public static MLContext LocalMLContext => _tMLContext ?? Create();


    [MethodImpl(MethodImplOptions.NoInlining)]
    private static MLContext Create()
    {
        return _tMLContext =
            _seed.HasValue ? new MLContext(_seed.Value) : new MLContext();
    }
}