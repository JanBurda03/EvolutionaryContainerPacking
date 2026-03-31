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
    /// Determines the population size used during evolution.
    /// <para>
    /// If <see cref="UseIndividualsAsRelative"/> is <c>false</c>,
    /// the value represents an absolute number of individuals.
    /// </para>
    /// <para>
    /// If <see cref="UseIndividualsAsRelative"/> is <c>true</c>,
    /// the value is interpreted as a multiplier and the resulting
    /// population size is computed relative to the problem size
    /// (e.g., number of boxes).
    /// </para>
    /// <para>
    /// Larger populations increase diversity but also computational cost.
    /// </para>
    /// </remarks>
    int Individuals,

    /// <summary>
    /// Indicates whether <see cref="Individuals"/> is interpreted as a relative value.
    /// </summary>
    /// <remarks>
    /// If set to <c>true</c>, the population size is computed as:
    /// <c>Individuals * problemSize</c>.
    /// Otherwise, <see cref="Individuals"/> is used as an absolute value.
    /// </remarks>
    bool UseIndividualsAsRelative,

    /// <summary>
    /// Maximum number of generations (iterations).
    /// </summary>
    /// <remarks>
    /// Evolution terminates when this generation limit is reached,
    /// unless stopped earlier by <see cref="TargetContainerCount"/>.
    /// </remarks>
    int NumberOfGenerations
);