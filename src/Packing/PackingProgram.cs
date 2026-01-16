public static class PackingProgram
{
    public static void Main()
    {
    }

    public static PackingRulesSolver CreateSolver(PackingInput packingInput, PackingSetting packingSetting)
    {
        return PackingRulesSolver.CreateSolver(packingInput, packingSetting);
    }

    public static IReadOnlyList<ContainerData> Solve(PackingRules packingVector, PackingInput packingInput, PackingSetting packingSetting)
    {
        var solver = PackingRulesSolver.CreateSolver(packingInput, packingSetting);
        return solver.Solve(packingVector);
    }

    public static IReadOnlyList<ContainerData> SolveRandom(PackingInput packingInput, PackingSetting packingSetting)
    {
        var packingVector = PackingRules.CreateRandom(packingInput.BoxPropertiesList.Count);
        return Solve(packingVector, packingInput, packingSetting);
    }

    public static int GetPackingVectorExpectedMinimalLength(PackingInput packingInput, PackingSetting packingSetting)
    {
        int i = 0;
        if (packingSetting.AllowRotations) 
        {
            i++;
        }
        if (packingSetting.SelectedPackingOrderHeuristic == null)
        {
            i++;
        }
        if(packingSetting.SelectedPlacementHeuristics.Length > 1)
        {
            i++;
        }
        return i * packingInput.BoxPropertiesList.Count;
    }
}