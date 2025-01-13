using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpFlrwTransformer(
    IIndividual model,
    DataManager data)
    : LogicGpTransformerBase<LogicGpFlrwModelOutput>(model, data)
{
    protected override List<LogicGpFlrwModelOutput> CreatePredictionData(
        IDataView input, string[] predictedClasses,
        DataManager dataManager)
    {
        var predictionData = new List<LogicGpFlrwModelOutput>();
        var labelColumn = input.GetColumnAsString(data.Label).ToList();

        for (var index = 0; index < labelColumn.Count; index++)
        {
            var predictedClass = predictedClasses[index];
            predictionData.Add(new LogicGpFlrwModelOutput
            {
                Y = labelColumn[index],
                PredictedLabel = predictedClass
            });
        }

        return predictionData;
    }
}