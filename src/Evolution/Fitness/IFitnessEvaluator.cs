namespace EvolutionaryContainerPacking.Evolution.Fitness;

using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Defines a fitness evaluation contract for individuals of type <typeparamref name="T"/>.
/// Provides methods for computing fitness, wrapping individuals with fitness values,
/// and comparing individuals or fitness scores.
/// </summary>
/// <typeparam name="T">Type of the individual being evaluated.</typeparam>
public interface IFitnessEvaluator<T>
{
    /// <summary>
    /// Computes the fitness value for a single individual.
    /// </summary>
    /// <param name="t">Individual to evaluate.</param>
    /// <returns>Fitness value.</returns>
    double EvaluateFitness(T t);

    /// <summary>
    /// Computes fitness values for a collection of individuals.
    /// </summary>
    /// <param name="t">Collection of individuals.</param>
    /// <returns>List of fitness values corresponding to the individuals.</returns>
    IReadOnlyList<double> EvaluateFitness(IReadOnlyList<T> t);

    /// <summary>
    /// Evaluates an individual and returns it wrapped together with its fitness value.
    /// </summary>
    /// <param name="t">Individual to evaluate.</param>
    /// <returns>An evaluated individual containing both the object and its fitness.</returns>
    EvaluatedIndividual<T> GenerateEvaluated(T t);

    /// <summary>
    /// Evaluates a collection of individuals and returns them wrapped with their fitness values.
    /// </summary>
    /// <param name="t">Collection of individuals.</param>
    /// <returns>List of evaluated individuals.</returns>
    IReadOnlyList<EvaluatedIndividual<T>> GenerateEvaluated(IReadOnlyList<T> t);

    /// <summary>
    /// Compares two individuals based on their fitness values.
    /// </summary>
    /// <param name="a">First individual.</param>
    /// <param name="b">Second individual.</param>
    /// <returns>
    /// Comparison result indicating which individual is better
    /// according to the optimization direction.
    /// </returns>
    int Compare(T a, T b);

    /// <summary>
    /// Compares two raw fitness values.
    /// </summary>
    /// <param name="a">First fitness value.</param>
    /// <param name="b">Second fitness value.</param>
    /// <returns>
    /// Comparison result indicating which fitness value is better
    /// according to the optimization direction.
    /// </returns>
    int Compare(double a, double b);

    /// <summary>
    /// Compares two evaluated individuals.
    /// </summary>
    /// <param name="a">First evaluated individual.</param>
    /// <param name="b">Second evaluated individual.</param>
    /// <returns>
    /// Comparison result indicating which evaluated individual is better
    /// according to the optimization direction.
    /// </returns>
    int Compare(EvaluatedIndividual<T> a, EvaluatedIndividual<T> b);

    /// <summary>
    /// Indicates whether the optimization problem is minimizing (true) or maximizing (false).
    /// </summary>
    bool Minimizing { get; }
}
