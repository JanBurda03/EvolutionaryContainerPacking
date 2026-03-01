namespace EvolutionaryContainerPacking.Evolution.EvolutionStatistics;

using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Writes on console statistical data during the evolutionary process.
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public class ConsoleOnlyEvolutionStatistics<T> : IEvolutionStatistics<T>
{
    /// <summary>
    /// Always returns an empty collection
    /// </summary>
    public IReadOnlyList<StatisticalData> EvolutionStatisticalData => Array.Empty<StatisticalData>();

    /// <summary>
    /// Writes statistics for the current iteration.
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

        Console.WriteLine($"Iteration {currentIteration}: Best = {best.Fitness}, Avg = {average}");
    }
}