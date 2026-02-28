namespace EvolutionaryContainerPacking.Evolution.EvolutionStatistics;

using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Collects statistical data during the evolutionary process.
/// <para>
/// Stores per-iteration information such as best fitness and average fitness,
/// which can later be used for analysis or visualization (e.g., charts).
/// </para>
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public class EvolutionStatistics<T> : IEvolutionStatistics<T>
{
    private readonly List<StatisticalData> _evolutionStatisticalData = new();

    /// <summary>
    /// Gets read-only statistical data collected during evolution.
    /// </summary>
    public IReadOnlyList<StatisticalData> EvolutionStatisticalData =>
        _evolutionStatisticalData.AsReadOnly();

    /// <summary>
    /// Updates statistics for the current iteration.
    /// </summary>
    /// <param name="currentIteration">Current iteration index.</param>
    /// <param name="population">Evaluated population.</param>
    /// <param name="best">Best individual of the current iteration.</param>
    public void Update(
        int currentIteration,
        IReadOnlyList<EvaluatedIndividual<T>> population,
        EvaluatedIndividual<T> best)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population must not be empty.", nameof(population));

        var fitnessValues = EvaluatedIndividual<T>.GetFitnesses(population);

        double average = fitnessValues.Average();

        _evolutionStatisticalData.Add(
            new StatisticalData(currentIteration, best.Fitness, average));

        // Consider replacing with logging abstraction if used in production
        Console.WriteLine(
            $"Iteration {currentIteration}: Best = {best.Fitness}, Avg = {average}");
    }
}


