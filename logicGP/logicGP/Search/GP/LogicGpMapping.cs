using Italbytz.Adapters.Algorithms.AI.Search.GP.Individuals;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Italbytz.Adapters.Algorithms.AI.Util.ML;
using Italbytz.Ports.Algorithms.AI.Search.GP.Individuals;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpMapping(IIndividual chosenIndividual)
{
    public Action<TSrc, TDst> GetMapping<TSrc, TDst>()
        where TSrc : class, new() where TDst : class, new()
    {
        return Map<TSrc, TDst>;
    }


    private void Map<TSrc, TDst>(TSrc arg1, TDst arg2)
        where TSrc : class, new() where TDst : class, new()
    {
        if (chosenIndividual is not Individual logicGpIndividual) return;
        if (logicGpIndividual.Genotype is not LogicGpGenotype logicGpGenotype)
            return;
        var gen = chosenIndividual.Genotype;
        var dst = ((LogicGpGenotype)gen).Predict<TSrc, TDst>(arg1);
        if (arg2 is ICustomMappingBinaryOutputSchema
                binaryDestinationSchema &&
            dst is ICustomMappingBinaryOutputSchema binaryPrediction)
        {
            binaryDestinationSchema.Probability =
                binaryPrediction.Probability;
            binaryDestinationSchema.Score =
                binaryPrediction.Score;
            binaryDestinationSchema.PredictedLabel =
                binaryPrediction.PredictedLabel;
        }
        else if
            (arg2 is ICustomMappingMulticlassOutputSchema
                 multiDestinationSchema &&
             dst is ICustomMappingMulticlassOutputSchema multiPrediction)
        {
            multiDestinationSchema.Probability =
                multiPrediction.Probability;
            multiDestinationSchema.Score =
                multiPrediction.Score;
            multiDestinationSchema.PredictedLabel =
                multiPrediction.PredictedLabel;
        }
    }
}