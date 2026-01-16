public class PackingRulesSolver
{
    private PackingRulesDecoder PackingVectorDecoder { get; init; }
    private PackingInput PackingInput { get; init; }
    public PackingRulesSolver(PackingRulesDecoder packingVectorDecoder, PackingInput packingInput)
    {
        PackingVectorDecoder = packingVectorDecoder;
        PackingInput = packingInput;
    }
    public IReadOnlyList<ContainerData> Solve(PackingRules packingVector)
    {
        
        IReadOnlyList<BoxToBePacked> boxesToBePacked = PackingVectorDecoder.Decode(packingVector);

        IBoxPacker BoxPacker = new BoxPacker(PackingInput.ContainerProperties);
        BoxPacker.PackBoxes(boxesToBePacked);
        var containers =  BoxPacker.ContainersData;


        return containers;
    }

    public static PackingRulesSolver CreateSolver(PackingInput packingInput, PackingSetting packingSetting)
    {
        PackingRulesDecoder packingVectorDecoder = PackingRulesDecoder.Create(packingSetting, packingInput);
        return new PackingRulesSolver(packingVectorDecoder, packingInput);
    }
}

