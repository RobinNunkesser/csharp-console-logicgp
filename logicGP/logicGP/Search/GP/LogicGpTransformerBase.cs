using Italbytz.Adapters.Algorithms.AI.Search.GP.Control;
using Italbytz.Adapters.Algorithms.AI.Search.GP.Fitness;
using Italbytz.Adapters.Algorithms.AI.Search.GP.SearchSpace;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public abstract class LogicGpTransformerBase<TModelOutput>(
    IIndividual model,
    DataManager data)
    : ITransformer where TModelOutput : class
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
        var predictedClasses =
            ((LogicGpGenotype)Model.Genotype).PredictedClasses;

        var predictionData =
            CreatePredictionData(input, predictedClasses, data);


        return mlContext.Data.LoadFromEnumerable(predictionData);
    }

    public IRowToRowMapper GetRowToRowMapper(DataViewSchema inputSchema)
    {
        throw new NotImplementedException();
    }

    public bool IsRowToRowMapper => false;

    protected abstract List<TModelOutput> CreatePredictionData(IDataView input,
        string[] predictedClasses, DataManager dataManager);
}