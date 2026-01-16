public static class EvolutionaryAlgorithms
{

    private static readonly Dictionary<
        string,
        Func<IReadOnlyList<PackingRules>, IFitnessEvaluator<PackingRules>, EvolutionaryAlgorithmSetting, IIterationStatistics<PackingRules>?, 
            IEvolutionary<PackingRules>>
    > EvolutionaryAlgorithmDictionary = new()
    {
        { "Elitist Genetic",
            (population, fitnessEvaluator, setting, evolutionData) =>
                new PackingVectorElitistGenetic(population, fitnessEvaluator, (ElitistGeneticSetting)setting, evolutionData)
        },
        { "Hill Descending",
            (population, fitnessEvaluator, setting, evolutionData) =>
                new PackingRulesHillDescending(population, fitnessEvaluator, (HillDescendingSetting)setting, evolutionData)
        },

    };

    public static string[] EvolutionaryAlgorithmsArray => EvolutionaryAlgorithmDictionary.Keys.ToArray();

    public static IEvolutionary<PackingRules> GetEvolutionaryAlgorithm(string name, IReadOnlyList<PackingRules> initialPopulation, IFitnessEvaluator<PackingRules> fitnessEvaluator, EvolutionaryAlgorithmSetting setting, IIterationStatistics<PackingRules>? evolutionData)
    {
        if (EvolutionaryAlgorithmDictionary.TryGetValue(name, out var factory))
            return factory(initialPopulation, fitnessEvaluator, setting, evolutionData);

        throw new ArgumentException($"Unknown evolutionary algorithm: {name}");
    }
}
