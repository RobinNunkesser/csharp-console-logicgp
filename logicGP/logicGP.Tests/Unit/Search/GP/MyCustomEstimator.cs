using logicGP.Tests.Data.Simulated;
using Microsoft.ML;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class MyCustomEstimator : IEstimator<ITransformer>
{
    public ITransformer Fit(IDataView input)
    {
        var mlContext = new MLContext();
        return mlContext.Transforms.CustomMapping(
            MyCustomMapper
                .GetMapping<SNPModelInput, BinaryClassificationSchema>(),
            null).Fit(input);
    }

    /// <summary>
    ///     This method cannot be implemented with reasonable effort because
    ///     of ML.NET only exposes the necessary API to "best friends".
    /// </summary>
    /// <param name="inputSchema"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public SchemaShape GetOutputSchema(SchemaShape inputSchema)
    {
        throw new NotImplementedException();
    }
}