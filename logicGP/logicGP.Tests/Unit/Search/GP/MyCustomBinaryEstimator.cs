using Italbytz.Adapters.Algorithms.AI.Util.ML;
using logicGP.Tests.Data.Simulated;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Italbytz.Adapters.Algorithms.AI.Search.GP;

public class MyCustomBinaryEstimator : IEstimator<ITransformer>
{
    public ITransformer Fit(IDataView input)
    {
        var mlContext = new MLContext();
        var labelColumn = input.Schema.GetColumnOrNull("Label");
        var labelColumnData = input
            .GetColumn<uint>("Label").ToList();
        return mlContext.Transforms.CustomMapping(
            MyCustomBinaryMapper
                .GetMapping<BinaryClassificationInputSchema,
                    BinaryClassificationOutputSchema>(),
            null).Fit(input);
    }

    /// <summary>
    ///     This method cannot be implemented with reasonable effort because
    ///     ML.NET only exposes the necessary API to "best friends".
    ///     Schema modifications therefore need to be done in CustomMappings
    /// </summary>
    /// <param name="inputSchema"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public SchemaShape GetOutputSchema(SchemaShape inputSchema)
    {
        var mlContext = new MLContext();
        var outputSchema = mlContext.Transforms.CustomMapping(
            MyCustomBinaryMapper
                .GetMapping<BinaryClassificationInputSchema,
                    BinaryClassificationOutputSchema>(),
            null).GetOutputSchema(inputSchema);

        return outputSchema;
    }
}