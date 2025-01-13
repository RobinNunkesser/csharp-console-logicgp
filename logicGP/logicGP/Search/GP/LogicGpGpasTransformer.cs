using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpGpasTransformer(
    IIndividual model,
    DataManager data)
    : LogicGpTransformerBase<LogicGpGpasModelOutput>(model, data)
{
    protected override List<LogicGpGpasModelOutput> CreatePredictionData(
        IDataView input, string[] predictedClasses,
        DataManager dataManager)
    {
        var predictionData = new List<LogicGpGpasModelOutput>();
        // Create a cursor to iterate through the rows
        using var cursor = input.GetRowCursor(input.Schema);
        // Get column indices
        var yIndex = cursor.Schema[data.Label];

        // Create delegates to access the values
        var yGetter = cursor.GetGetter<float>(yIndex);

        // Variables to hold the values
        float y = 0;

        var index = 0;
        // Iterate through the rows
        while (cursor.MoveNext())
        {
            yGetter(ref y);
            var predictedClass = predictedClasses[index];
            // TODO: Handle other types
            var score = predictedClass.Equals("1") ? 1 : 0;


            predictionData.Add(new LogicGpGpasModelOutput
            {
                Y = (uint)y,

                Score = new[] { score, (float)(1 - score) },
                PredictedLabel = predictedClass
            });
            index++;
        }

        return predictionData;
    }
}