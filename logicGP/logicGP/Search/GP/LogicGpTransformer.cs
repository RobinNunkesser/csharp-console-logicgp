using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class LogicGpTransformer(IIndividual model, DataManager data)
    : ITransformer
{
    public IIndividual Model { get; } = model;

    public void Save(ModelSaveContext ctx)
    {
        throw new NotImplementedException();
    }

    public DataViewSchema GetOutputSchema(DataViewSchema inputSchema)
    {
        throw new NotImplementedException();
    }

    public IDataView Transform(IDataView input)
    {
        foreach (var literal in data.Literals)
            literal.GeneratePredictions(
                input.GetColumnAsString(literal.Label).ToList());
        ((LogicGpGenotype)Model.Genotype)
            .UpdatePredictionsRecursively();
        var mlContext = new MLContext();
        var predictionData = new List<LogicGpModelOutput>();
        var predictedClasses =
            ((LogicGpGenotype)Model.Genotype).PredictedClasses;

        // Create a cursor to iterate through the rows
        using (var cursor = input.GetRowCursor(input.Schema))
        {
            // Get column indices
            var yIndex = cursor.Schema["y"];

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


                predictionData.Add(new LogicGpModelOutput
                {
                    Y = (uint)y,

                    Score = new[] { score, (float)(1 - score) },
                    PredictedLabel = predictedClass
                });
                index++;
            }
        }

        return mlContext.Data.LoadFromEnumerable(predictionData);
    }

    public IRowToRowMapper GetRowToRowMapper(DataViewSchema inputSchema)
    {
        throw new NotImplementedException();
    }

    public bool IsRowToRowMapper => false;
}