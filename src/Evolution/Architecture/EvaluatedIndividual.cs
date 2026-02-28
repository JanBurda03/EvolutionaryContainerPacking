namespace EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Represents an individual together with its fitness value.
/// </summary>
/// <typeparam name="T">
/// Type of the individual representation.
/// </typeparam>
public class EvaluatedIndividual<T>
{
    /// <summary>
    /// Underlying individual.
    /// </summary>
    public T Individual { get; }

    /// <summary>
    /// Fitness value of the individual.
    /// </summary>
    public double Fitness { get; }

    public EvaluatedIndividual(T individual, double fitness)
    {
        Individual = individual;
        Fitness = fitness;
    }

    /// <summary>
    /// Combines individuals and fitness values into evaluated individuals.
    /// </summary>
    public static IReadOnlyList<EvaluatedIndividual<T>> Merge(
        IReadOnlyList<T> individuals,
        IReadOnlyList<double> fitnesses)
    {
        if (fitnesses.Count != individuals.Count)
        {
            throw new ArgumentException(
                "List of individuals and list of fitnesses must have the same length");
        }

        EvaluatedIndividual<T>[] evaluated =
            new EvaluatedIndividual<T>[individuals.Count];

        for (int i = 0; i < individuals.Count; i++)
        {
            evaluated[i] = Merge(individuals[i], fitnesses[i]);
        }

        return evaluated;
    }

    /// <summary>
    /// Creates a single evaluated individual.
    /// </summary>
    public static EvaluatedIndividual<T> Merge(T individual, double fitness)
        => new EvaluatedIndividual<T>(individual, fitness);

    /// <summary>
    /// Extracts fitness values from evaluated individuals.
    /// </summary>
    public static IReadOnlyList<double> GetFitnesses(
        IReadOnlyList<EvaluatedIndividual<T>> evaluated)
    {
        double[] fitnesses = new double[evaluated.Count];

        for (int i = 0; i < evaluated.Count; i++)
        {
            fitnesses[i] = evaluated[i].Fitness;
        }

        return fitnesses;
    }

    /// <summary>
    /// Extracts individuals from evaluated individuals.
    /// </summary>
    public static IReadOnlyList<T> GetIndividuals(
        IReadOnlyList<EvaluatedIndividual<T>> evaluated)
    {
        T[] individuals = new T[evaluated.Count];

        for (int i = 0; i < evaluated.Count; i++)
        {
            individuals[i] = evaluated[i].Individual;
        }

        return individuals;
    }
}
