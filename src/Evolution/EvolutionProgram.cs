
using System.Text;

public static class EvolutionProgram
{
    public static void Main() { }

    public static IReadOnlyList<ContainerData> Run(ProgramSetting setting, PackingInput packingInput)
    {
        var initialPopulation = CreateInitialPopulation(packingInput, setting.PackingSetting, setting.EvolutionSetting.NumberOfIndividuals);
        var evaluator = PackingRulesFitnessEvaluator.Create(packingInput, setting.PackingSetting);

        var evolutionStatistics = new EvolutionStatistics<PackingRules>();

        var evolutionary = EvolutionaryAlgorithms.GetEvolutionaryAlgorithm(setting.EvolutionSetting.AlgorithmName, initialPopulation, evaluator, setting.EvolutionAlgorithmSetting, evolutionStatistics);


        evolutionary.Evolve(setting.EvolutionSetting.NumberOfGenerations);
        var best = evolutionary.GlobalBest.Individual;

        var solution = PackingProgram.Solve(best, packingInput, setting.PackingSetting);

        PackingValidityChecker(solution);

        SaveStatisticsCsv(evolutionStatistics, setting.EvolutionSetting.IterationStatisticsFile);

        return solution;
    }

    public static void SaveStatisticsCsv(EvolutionStatistics<PackingRules> s, string fileName)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Generation;Best;Average");

        foreach (var i in s.IterationScore)
        {
            sb.AppendLine(
                $"{i.IterationNumber};{i.BestScore};{i.AverageScore}"
            );
        }

        File.WriteAllText(fileName, sb.ToString());
    }





    public static IReadOnlyList<PackingRules> CreateInitialPopulation(PackingInput packingInput, PackingSetting packingSetting, int populationSize)
    {
        IPopulationFactory<PackingRules> packingVectorFactory = new PackingRulesPopulationFactory(PackingProgram.GetPackingVectorExpectedMinimalLength(packingInput, packingSetting));
        return packingVectorFactory.CreatePopulation(populationSize);
        
    }

    public static void PackingValidityChecker(IReadOnlyList<ContainerData> containers)
    {

        foreach (ContainerData container in containers)
        {
            var containerRegion = container.ContainerProperties.Sizes.ToRegion(new Coordinates(0, 0, 0));

            IReadOnlyList<PackedBox> packedBoxes = container.PackedBoxes;

            for (int i = 0; i < packedBoxes.Count; i++) 
            { 
                for (int j = i+1;  j < packedBoxes.Count; j++)
                {
                    var box1 = packedBoxes[i];
                    var box2 = packedBoxes[j];

                    if (box1.PlacementInfo.OccupiedRegion.IntersectsWith(box2.PlacementInfo.OccupiedRegion) || !box1.PlacementInfo.OccupiedRegion.IsSubregionOf(containerRegion) || !box2.PlacementInfo.OccupiedRegion.IsSubregionOf(containerRegion))
                    {
                        throw new Exception($"Fatal packing program error, in container {container.ID}, box {box1} intersects with {box2}!");
                    }
                }
            }
        }
    }
}

