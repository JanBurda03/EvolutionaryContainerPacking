namespace EvolutionaryContainerPacking.Evolution.EvolutionStatistics;

using EvolutionaryContainerPacking.Evolution.Architecture;
using System.Diagnostics;

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

    // Stopwatch se NEspouští při vytvoření objektu
    private readonly Stopwatch _stopwatch = new();
    private bool _timerStarted = false;

    /// <summary>
    /// Gets read-only statistical data collected during evolution.
    /// </summary>
    public IReadOnlyList<StatisticalData> EvolutionStatisticalData => _evolutionStatisticalData.AsReadOnly();

    /// <summary>
    /// Updates statistics for the current iteration.
    /// </summary>
    public void Update(
        int currentIteration,
        IReadOnlyList<EvaluatedIndividual<T>> population,
        EvaluatedIndividual<T> best)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population must not be empty.", nameof(population));

        // Spustí časovač až při prvním update
        if (!_timerStarted)
        {
            _stopwatch.Start();
            _timerStarted = true;
        }

        var fitnessValues = EvaluatedIndividual<T>.GetFitnesses(population);
        double average = fitnessValues.Average();

        // mezičas od prvního update (v sekundách)
        double elapsedSeconds = _stopwatch.Elapsed.TotalSeconds;

        _evolutionStatisticalData.Add(
            new StatisticalData(currentIteration, best.Fitness, average, elapsedSeconds)
        );

        Console.WriteLine(
            $"Iteration {currentIteration}: Best = {best.Fitness}, Avg = {average}, Time = {elapsedSeconds:F2}s"
        );
    }
}


