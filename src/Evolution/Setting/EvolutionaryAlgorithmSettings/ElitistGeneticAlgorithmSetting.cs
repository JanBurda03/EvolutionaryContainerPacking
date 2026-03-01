namespace EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;


/// <summary>
/// Configuration for the Elitist Genetic Algorithm with optional memetic extension.
/// </summary>
/// <remarks>
/// This algorithm preserves an elite subset of the population across generations.
/// New individuals are created using biased uniform crossover, mutation,
/// and optional injection of random solutions.
/// 
/// A memetic extension can be enabled by specifying
/// <see cref="HillClimbingIterations"/> greater than zero.
/// If set to zero, no local search is applied.
/// </remarks>
public sealed record class ElitistGeneticAlgorithmSetting(

    /// <summary>
    /// Optional target fitness value for early stopping of the evolutionary algorithm.
    /// </summary>
    double? TargetFitness,

    /// <summary>
    /// Total number of individuals in the population.
    /// </summary>
    int NumberOfIndividuals,

    /// <summary>
    /// Maximum number of generations.
    /// </summary>
    int NumberOfGenerations,

    /// <summary>
    /// Percentage of elites individuals.
    /// </summary>
    /// <remarks>
    /// Must be in range [0,1].
    /// 
    /// 0 → no elites
    /// 0.1 → 10% of population considered as elites  
    /// 1 → everyone individual
    /// </remarks>
    double PercentageOfEliteIndividuals,

    /// <summary>
    /// Percentage of elements inherited from the elite parent
    /// during uniform crossover.
    /// </summary>
    /// <remarks>
    /// Controls exploration.
    /// Higher values increase bias toward elite traits.
    /// Must be in range [0,1].
    /// </remarks>
    double PercentageOfElementsFromElite,

    /// <summary>
    /// Percentage of elements mutated in each offspring.
    /// </summary>
    /// <remarks>
    /// Defines mutation strength.
    /// Must be in range [0,1].
    /// </remarks>
    double PercentageOfElementsMutated,

    /// <summary>
    /// Percentage of entirely new random individuals introduced
    /// into the population each generation.
    /// </summary>
    /// <remarks>
    /// Must be in range [0,1].
    /// 
    /// 0 → no random injection  
    /// 0.1 → 10% of population replaced with random individuals  
    /// 1 → full random restart each generation
    /// </remarks>
    double PercentageOfRandomIndividuals,


    /// <summary>
    /// Number of hill climbing iterations applied to each offspring.
    /// </summary>
    /// <remarks>
    /// 0 disables the memetic extension.
    /// </remarks>
    int HillClimbingIterations,

    /// <summary>
    /// Average number of elements mutated during
    /// each hill climbing step.
    /// </summary>
    /// <remarks>
    /// Only used if <see cref="HillClimbingIterations"/> is greater than zero.
    /// </remarks>
    double HillClimbingAverageElementsMutated

) : EvolutionaryAlgorithmSetting(
        TargetFitness,
        NumberOfIndividuals,
        NumberOfGenerations
    );