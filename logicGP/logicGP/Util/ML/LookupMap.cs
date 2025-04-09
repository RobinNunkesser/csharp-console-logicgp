namespace Italbytz.Adapters.Algorithms.AI.Util.ML;

public class LookupMap<TKey>(TKey key)
{
    public TKey Key { get; set; } = key;

    public static Dictionary<int, string> KeyToValueMap(
        LookupMap<TKey>[] lookupData)
    {
        var lookupMap = new Dictionary<int, string>();
        for (var i = 0; i < lookupData.Length; i++)
            lookupMap[i + 1] = lookupData[i].Key?.ToString() ?? string.Empty;
        return lookupMap;
    }
}