namespace EvolutionaryContainerPacking.Evolution.Fitness;

using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Provides comparison logic for fitness values and evaluated individuals,
/// supporting both minimization and maximization objectives.
/// </summary>
public class FitnessComparer
{
    public readonly bool Minimizing;

    /// <summary>
    /// Initializes a new instance of the <see cref="FitnessComparer"/> class.
    /// </summary>
    /// <param name="minimizing">
    /// If true, lower fitness values are considered better (minimization problem).
    /// If false, higher fitness values are considered better (maximization problem).
    /// </param>
    public FitnessComparer(bool minimizing)
    {
        Minimizing = minimizing;
    }

    /// <summary>
    /// Compares two raw fitness values.
    /// </summary>
    /// <param name="x">First fitness value.</param>
    /// <param name="y">Second fitness value.</param>
    /// <returns>
    /// A signed integer indicating relative quality:
    /// negative if x is better than y,
    /// positive if x is worse than y,
    /// zero if they are equal,
    /// according to the optimization direction.
    /// </returns>
    public int Compare(double x, double y)
    {
        int comparison = x.CompareTo(y);
        return Minimizing ? comparison : -comparison;
    }

    /// <summary>
    /// Compares two evaluated individuals based on their fitness values.
    /// </summary>
    /// <typeparam name="T">Type of the evaluated individual.</typeparam>
    /// <param name="x">First evaluated individual.</param>
    /// <param name="y">Second evaluated individual.</param>
    /// <returns>
    /// A signed integer indicating which individual is better
    /// according to the optimization direction.
    /// </returns>
    public int Compare<T>(EvaluatedIndividual<T> x, EvaluatedIndividual<T> y)
    {
        int comparison = x.Fitness.CompareTo(y.Fitness);
        return Minimizing ? comparison : -comparison;
    }
}