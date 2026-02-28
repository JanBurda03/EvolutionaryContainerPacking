namespace EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;

/// <summary>
/// Represents base configuration settings for an evolutionary algorithm.
/// </summary>
/// <remarks>
/// Contains general termination and population parameters shared by all
/// evolutionary algorithm implementations. Specific algorithms may derive
/// from this class and extend it with additional parameters.
/// </remarks>
public record class EvolutionaryAlgorithmSetting(

    /// <summary>
    /// Optional target fitness value for early stopping of the evolutionary algorithm.
    /// </summary>
    /// <remarks>
    /// If specified, the evolutionary process terminates early
    /// as soon as a solution is found with a fitness **lower than** this value.
    /// <para>
    /// The comparison is non-inclusive: specifying 8.0 means the algorithm stops
    /// only when the fitness is strictly less than 8.0, not equal to 8.0 
    /// (only 7 containers are accepted, not 8).
    /// </para>
    /// <para>
    /// If null, no fitness-based early stopping is applied.
    /// </para>
    /// </remarks>
    double? TargetFitness,

    /// <summary>
    /// Number of individuals in the population.
    /// </summary>
    /// <remarks>
    /// Controls population size maintained during evolution.
    /// Larger populations increase diversity but also computational cost.
    /// </remarks>
    int NumberOfIndividuals,

    /// <summary>
    /// Maximum number of generations (iterations).
    /// </summary>
    /// <remarks>
    /// Evolution terminates when this generation limit is reached,
    /// unless stopped earlier by <see cref="TargetContainerCount"/>.
    /// </remarks>
    int NumberOfGenerations
);