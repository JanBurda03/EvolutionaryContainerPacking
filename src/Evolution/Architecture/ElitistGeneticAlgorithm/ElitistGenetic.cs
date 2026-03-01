namespace EvolutionaryContainerPacking.Evolution.Architecture.ElitistGenetic;

using EvolutionaryContainerPacking.Evolution.Architecture.Mutation;
using EvolutionaryContainerPacking.Evolution.Architecture.Crossover;
using EvolutionaryContainerPacking.Evolution.Architecture.Selection;
using EvolutionaryContainerPacking.Evolution.Architecture.Population;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

/// <summary>
/// Implements an elitist genetic algorithm for evolving a population of individuals.
/// Maintains elite individuals, applies crossover and mutation, and introduces
/// random individuals to maintain diversity.
/// </summary>
/// <typeparam name="T">Type of the individual (e.g., PackingRules).</typeparam>
public class ElitistGeneticAlgorithm<T> : EvolutionaryBase<T>
{
    // Mutation operator for generating variations
    protected readonly IMutation<T> _mutation;

    // Crossover operator for combining two individuals
    protected readonly ICrossover<T> _crossover;

    // Selection mechanism for picking individuals randomly
    protected readonly ISelection<T> _randomSelection;

    // Number of entirely new random individuals to add per generation
    protected readonly int _numberOfRandomIndividuals;

    // Number of elite individuals to preserve each generation
    protected readonly int _numberOfEliteIndividuals;

    // Number of individuals to select and apply crossover/mutation
    protected readonly int _numberOfSelectedIndividuals;

    /// <summary>
    /// Constructs the elitist genetic algorithm with the specified settings and operators.
    /// </summary>
    public ElitistGeneticAlgorithm(
        IPopulationFactory<T> populationFactory,
        IFitnessEvaluator<T> fitnessEvaluator,
        IElitism<T> elitism,
        ICrossover<T> crossover,
        IMutation<T> mutation,
        ElitistGeneticAlgorithmSetting setting,
        IEvolutionStatistics<T> evolutionStatistics)
        : base(populationFactory, fitnessEvaluator, elitism, setting, evolutionStatistics)
    {
        // Calculate the number of individuals in each category
        _numberOfRandomIndividuals = (int)Math.Ceiling(setting.PercentageOfRandomIndividuals * Population.Count);
        _numberOfEliteIndividuals = (int)Math.Ceiling(setting.PercentageOfEliteIndividuals * Population.Count);
        _numberOfSelectedIndividuals = Population.Count - _numberOfRandomIndividuals - _numberOfEliteIndividuals;

        _crossover = crossover;
        _mutation = mutation;

        _randomSelection = new RandomSelection<T>();
    }

    /// <summary>
    /// Produces the next generation of the population.
    /// </summary>
    protected override void NextGeneration()
    {
        // preserve elite individuals
        var elites = _elitism.GetElites(Population, _numberOfEliteIndividuals);

        // select individuals from elites and non-elites for crossover
        var selectedElites = _randomSelection.Select(elites, _numberOfSelectedIndividuals);
        var selected = _randomSelection.Select(Population, _numberOfSelectedIndividuals);

        // apply crossover between elite and non-elite selections
        var crossovered = _crossover.Crossover(selectedElites, selected);

        // apply mutation to the crossovered individuals
        var mutated = _mutation.Mutate(crossovered);

        // add entirely new random individuals to maintain diversity
        var added = _populationFactory.CreatePopulation(_numberOfRandomIndividuals);

        // combine mutated and new individuals
        var result = mutated.Concat(added).ToList();

        // evaluate the new generation and append preserved elites
        Population = (_fitnessEvaluator.GenerateEvaluated(result)).Concat(elites).ToList();
    }
}



