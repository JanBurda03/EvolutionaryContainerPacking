namespace EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Represents a generic evolutionary algorithm.
/// </summary>
/// <typeparam name="T">
/// Type of the individual representation.
/// </typeparam>
public interface IEvolutionary<T>
{
    /// <summary>
    /// Current population.
    /// </summary>
    IReadOnlyList<EvaluatedIndividual<T>> Population { get; }

    /// <summary>
    /// Best individual found so far.
    /// </summary>
    EvaluatedIndividual<T> Best { get; }

    /// <summary>
    /// Runs the algorithm for the specified number of generations.
    /// </summary>
    T Run(int numberOfGenerationsPerRun);

    /// <summary>
    /// Runs the algorithm using its configured number of generations.
    /// </summary>
    T Run();
}