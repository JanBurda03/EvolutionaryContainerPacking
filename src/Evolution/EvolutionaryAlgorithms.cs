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
/// Factory responsible for creating evolutionary algorithm instances
/// based on the provided algorithm setting type.
/// </summary>
/// <remarks>
/// The specific algorithm implementation is determined by the concrete
/// type of <see cref="EvolutionaryAlgorithmSetting"/>.
/// </remarks>
public static class EvolutionaryAlgorithms
{
    /// <summary>
    /// Creates an evolutionary algorithm instance corresponding
    /// to the provided configuration object.
    /// </summary>
    /// <param name="populationFactory">
    /// Factory used to generate initial populations.
    /// </param>
    /// <param name="fitnessEvaluator">
    /// Evaluates the fitness of individuals.
    /// </param>
    /// <param name="setting">
    /// Configuration object determining which algorithm is used.
    /// </param>
    /// <param name="evolutionStatistics">
    /// Collector for evolution statistics.
    /// </param>
    /// <returns>
    /// Configured evolutionary algorithm instance.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the provided setting type is not supported.
    /// </exception>
    public static IEvolutionary<PackingRules> GetEvolutionaryAlgorithm(
        IPopulationFactory<PackingRules> populationFactory,
        IFitnessEvaluator<PackingRules> fitnessEvaluator,
        EvolutionaryAlgorithmSetting setting,
        IEvolutionStatistics<PackingRules> evolutionStatistics)
    {
        return setting switch
        {
            ElitistGeneticAlgorithmSetting ega =>
                new PackingRulesElitistGeneticAlgorithm(
                    populationFactory,
                    fitnessEvaluator,
                    ega,
                    evolutionStatistics),

            ProbabilisticHillClimbingSetting phc =>
                new PackingRulesProbabilisticHillClimbing(
                    populationFactory,
                    fitnessEvaluator,
                    phc,
                    evolutionStatistics),

            _ => throw new ArgumentException(
                $"Unsupported evolutionary algorithm setting type: {setting.GetType().Name}")
        };
    }
}