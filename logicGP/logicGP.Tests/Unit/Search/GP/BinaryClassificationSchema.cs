namespace logicGP.Tests.Data.Simulated;

public class BinaryClassificationSchema
{
    public float[] Features { get; set; }
    public float PredictedLabel { get; set; }
    public float Score { get; set; }
    public float Probability { get; set; }
}