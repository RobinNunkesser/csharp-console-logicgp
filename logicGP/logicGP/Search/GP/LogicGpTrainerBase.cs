using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public abstract class
    LogicGpTrainerBase<TTransformer> : IEstimator<TTransformer>
    where TTransformer : ITransformer

{
    public required string Label { get; set; }

    public TTransformer Fit(IDataView input)
    {
        return ConcreteFit(input, Label);
    }

    public SchemaShape GetOutputSchema(SchemaShape inputSchema)
    {
        throw new NotImplementedException();
    }

    protected abstract TTransformer ConcreteFit(IDataView input, string label);
}