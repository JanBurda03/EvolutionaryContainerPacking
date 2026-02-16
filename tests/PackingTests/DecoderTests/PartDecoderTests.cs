public class PartDecoderTests
{
    [Fact]
    public void PartDecoderTest()
    {
        double[] values = [0.0, 0.21, 0.5, 0.7, 0.99];
        PackingVectorUsingPartDecoder<double> decoder = new PackingVectorUsingPartDecoder<double>(values);

        for (int i = 0; i < values.Length; i++) 
        {
            Assert.Equal(values[i], decoder.Decode((PackingRulesCell)values[i]));
        }

    }
}