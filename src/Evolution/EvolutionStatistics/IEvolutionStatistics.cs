namespace EvolutionaryContainerPacking.Evolution.EvolutionStatistics;

using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Provides statistical tracking during the evolutionary process.
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public interface IEvolutionStatistics<T>
{
    /// <summary>
    /// Gets collected statistical data for all completed iterations.
    /// </summary>
    IReadOnlyList<StatisticalData> EvolutionStatisticalData { get; }

    /// <summary>
    /// Updates statistics for the current iteration.
    /// </summary>
    /// <param name="currentIteration">Current iteration index.</param>
    /// <param name="population">Evaluated population.</param>
    /// <param name="best">Best individual of the current iteration.</param>
    void Update(
        int currentIteration,
        IReadOnlyList<EvaluatedIndividual<T>> population,
        EvaluatedIndividual<T> best);
}