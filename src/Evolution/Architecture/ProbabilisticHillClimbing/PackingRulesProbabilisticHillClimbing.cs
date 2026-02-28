namespace EvolutionaryContainerPacking.Evolution.Architecture.ProbabilisticHillClimbing;

using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Packing.Rules;
using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Evolution.Architecture.Population;
using EvolutionaryContainerPacking.Evolution.Architecture.Mutation;
using EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

/// <summary>
/// Probabilistic hill climbing specialized for <see cref="PackingRules"/>.
/// </summary>
public class PackingRulesProbabilisticHillClimbing : ProbabilisticHillClimbing<PackingRules>
{
    public PackingRulesProbabilisticHillClimbing(
        IPopulationFactory<PackingRules> populationFactory, 
        IFitnessEvaluator<PackingRules> fitnessEvaluator, 
        ProbabilisticHillClimbingSetting setting,
        IEvolutionStatistics<PackingRules> evolutionStatistics)

        : base(populationFactory, fitnessEvaluator, new Elitism<PackingRules>(fitnessEvaluator.Minimizing), new PackingRulesMutation(setting.AverageElementsChanged), setting, evolutionStatistics)
    { }
}