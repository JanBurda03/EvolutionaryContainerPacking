namespace EvolutionaryContainerPacking.Evolution.EvolutionStatistics;

using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Empty implementation of <see cref="IEvolutionStatistics{T}"/>.
/// <para>
/// Used when statistical tracking is not required.
/// Implements the Null Object pattern to avoid null checks.
/// </para>
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public sealed class EmptyStatistics<T> : IEvolutionStatistics<T>
{
    /// <summary>
    /// Always returns an empty collection
    /// </summary>
    public IReadOnlyList<StatisticalData> EvolutionStatisticalData =>
        Array.Empty<StatisticalData>();

    /// <summary>
    /// Does nothing
    /// </summary>
    public void Update(
        int currentIteration,
        IReadOnlyList<EvaluatedIndividual<T>> population,
        EvaluatedIndividual<T> best)
    {
        // Intentionally empty
    }
}