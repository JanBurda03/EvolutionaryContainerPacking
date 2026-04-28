namespace EvolutionaryContainerPacking.Evolution.Architecture.ElitistGenetic;

using EvolutionaryContainerPacking.Evolution.Architecture.Population;
using EvolutionaryContainerPacking.Evolution.Architecture.Memetic;
using EvolutionaryContainerPacking.Evolution.Architecture.Elitism;
using EvolutionaryContainerPacking.Evolution.Architecture.Crossover;
using EvolutionaryContainerPacking.Evolution.Architecture.Mutation;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Packing.Rules;

public class PackingRulesElitistGeneticAlgorithm : ElitistGeneticAlgorithm<PackingRules>
{    
    public PackingRulesElitistGeneticAlgorithm(
        IPopulationFactory<PackingRules> populationFactory,
        IFitnessEvaluator<PackingRules> fitnessEvaluator,
        ElitistGeneticAlgorithmSetting setting,
        IEvolutionStatistics<PackingRules> evolutionStatistics
        ) 
        
        : base(
            populationFactory, 
            fitnessEvaluator, 
            new Elitism<PackingRules>(fitnessEvaluator.Minimizing), 
            new HillClimbingMemetic<PackingRules>(new PackingRulesMutation(setting.HillClimbingPercentageOfElementsMutated), fitnessEvaluator, setting.HillClimbingIterations), 
            new PackingRulesUniformCrossover(setting.PercentageOfElementsFromElite), 
            new PackingRulesMutation(setting.PercentageOfElementsMutated),
            setting, 
            evolutionStatistics)
    {}
}