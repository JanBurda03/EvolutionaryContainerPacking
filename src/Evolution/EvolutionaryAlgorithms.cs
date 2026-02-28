namespace EvolutionaryContainerPacking.Evolution;

using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Architecture.Population;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Evolution.Architecture.ProbabilisticHillClimbing;
using EvolutionaryContainerPacking.Evolution.Architecture.ElitistGenetic;
using EvolutionaryContainerPacking.Evolution.Architecture;
using EvolutionaryContainerPacking.Packing.Rules;

/// <summary>
/// Factory for creating evolutionary algorithm instances
/// based on a string identifier.
/// </summary>
public static class EvolutionaryAlgorithms
{
    private static readonly Dictionary<
        string,
        Func<
            IPopulationFactory<PackingRules>,
            IFitnessEvaluator<PackingRules>,
            EvolutionaryAlgorithmSetting,
            IEvolutionStatistics<PackingRules>,
            IEvolutionary<PackingRules>
        >
    > EvolutionaryAlgorithmDictionary = new()
    {
        {
            "Elitist Genetic",
            (populationFactory, fitnessEvaluator, setting, evolutionStatistics) =>
                new PackingRulesElitistGeneticAlgorithm(
                    populationFactory,
                    fitnessEvaluator,
                    (ElitistGeneticAlgorithmSetting)setting,
                    evolutionStatistics)
        },
        {
            "Hill Climbing",
            (populationFactory, fitnessEvaluator, setting, evolutionStatistics) =>
                new PackingRulesProbabilisticHillClimbing(
                    populationFactory,
                    fitnessEvaluator,
                    (ProbabilisticHillClimbingSetting)setting,
                    evolutionStatistics)
        },
    };

    /// <summary>
    /// Gets available algorithm names.
    /// </summary>
    public static string[] EvolutionaryAlgorithmsArray
        => EvolutionaryAlgorithmDictionary.Keys.ToArray();

    /// <summary>
    /// Creates the selected evolutionary algorithm.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if the algorithm name is unknown
    /// or if the provided setting type does not match.
    /// </exception>
    public static IEvolutionary<PackingRules> GetEvolutionaryAlgorithm(
        string algorithmName,
        IPopulationFactory<PackingRules> populationFactory,
        IFitnessEvaluator<PackingRules> fitnessEvaluator,
        EvolutionaryAlgorithmSetting setting,
        IEvolutionStatistics<PackingRules> evolutionStatistics)
    {
        if (EvolutionaryAlgorithmDictionary.TryGetValue(algorithmName, out var factory))
        {
            try
            {
                return factory(populationFactory, fitnessEvaluator, setting, evolutionStatistics);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(
                    $"Algorithm '{algorithmName}' requires a different setting type.");
            }
        }

        throw new ArgumentException($"Unknown evolutionary algorithm: {algorithmName}");
    }
}