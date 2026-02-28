namespace EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;

/// <summary>
/// Configuration for a population-based probabilistic hill climbing algorithm.
/// </summary>
/// <remarks>
/// This algorithm maintains a population of solutions and performs local mutations on each individual.
///
/// Worse solutions may be accepted with a probability that gradually decreases
/// over the course of evolution, controlled by <see cref="AcceptanceDecayFactor"/>.
/// 
/// At the beginning of evolution, worse solutions are accepted more frequently (according to <see cref="StartAcceptanceProbability"/>),
/// while towards the end, only better solutions are typically accepted (approaching <see cref="EndAcceptanceProbability"/>).
/// 
/// The <see cref="AcceptanceDecayFactor"/> specifies how much the acceptance probability
/// is multiplied in each generation to produce a gradual decay from start to end.
/// The actual probability calculation is performed in the evolutionary algorithm logic.
/// </remarks>
public record class ProbabilisticHillClimbingSetting(

    /// <summary>
    /// Optional target fitness value for early stopping of the evolutionary algorithm.
    /// </summary>
    double? TargetFitness,

    /// <summary>
    /// Number of individuals in the population.
    /// </summary>
    int NumberOfIndividuals,

    /// <summary>
    /// Maximum number of generations (iterations).
    /// </summary>
    int NumberOfGenerations,

    /// <summary>
    /// Average number of elements (genes) modified when generating a neighboring solution.
    /// </summary>
    /// <remarks>
    /// Higher values increase exploration but may destabilize convergence.
    /// </remarks>
    double AverageElementsChanged,

    /// <summary>
    /// Probability of accepting a worse solution at the beginning of evolution.
    /// </summary>
    /// <remarks>
    /// Must be in range [0, 1].
    /// 1.0 → always accept worse solutions (pure exploration).
    /// 0.0 → purely greedy hill climbing.
    /// </remarks>
    double StartAcceptanceProbability,

    /// <summary>
    /// Probability of accepting a worse solution at the end of evolution.
    /// </summary>
    /// <remarks>
    /// Must be in range [0, 1].
    /// Typically lower than <see cref="StartAcceptanceProbability"/>,
    /// enabling gradual transition from exploration to exploitation.
    /// </remarks>
    double EndAcceptanceProbability,

    /// <summary>
    /// Factor by which the acceptance probability is multiplied each generation.
    /// </summary>
    /// <remarks>
    /// Example: 0.99 → acceptance probability decays by 1% each generation.
    /// </remarks>
    double AcceptanceDecayFactor

) : EvolutionaryAlgorithmSetting(
        TargetFitness,
        NumberOfIndividuals,
        NumberOfGenerations
    );