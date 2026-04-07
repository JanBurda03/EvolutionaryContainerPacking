namespace EvolutionaryContainerPacking.Evolution.Architecture.Memetic;

using EvolutionaryContainerPacking.Evolution.Architecture.Mutation;
using EvolutionaryContainerPacking.Evolution.Architecture;
using EvolutionaryContainerPacking.Evolution.Fitness;


/// <summary>
/// Deterministic hill climbing memetic operator working directly
/// with evaluated individuals.
///
/// For each individual:
///     - generate neighbor using mutation
///     - evaluate neighbor
///     - accept only if strictly better
///     - repeat for fixed number of iterations
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public class HillClimbingMemetic<T> : IMemetic<T>
{
    private readonly IMutation<T> _mutation;
    private readonly IFitnessEvaluator<T> _fitnessEvaluator;
    private readonly int _iterations;

    /// <summary>
    /// Initializes hill climbing memetic operator.
    /// </summary>
    /// <param name="mutation">Mutation operator used to generate neighbors.</param>
    /// <param name="fitnessEvaluator">Fitness evaluator used for comparison.</param>
    /// <param name="iterations">Number of local search steps per individual.</param>
    public HillClimbingMemetic(
        IMutation<T> mutation,
        IFitnessEvaluator<T> fitnessEvaluator,
        int iterations)
    {
        if (iterations < 0)
            throw new ArgumentOutOfRangeException(nameof(iterations), "Must be >= 0.");

        _mutation = mutation;
        _fitnessEvaluator = fitnessEvaluator;
        _iterations = iterations;
    }

    /// <summary>
    /// Applies hill climbing to a single non-evaluated individual.
    /// </summary>
    public EvaluatedIndividual<T> Memetic(T individual)
    {
        return Memetic(_fitnessEvaluator.GenerateEvaluated(individual));
    }


    /// <summary>
    /// Applies hill climbing to a single evaluated individual.
    /// </summary>
    public EvaluatedIndividual<T> Memetic(EvaluatedIndividual<T> individual)
    {

        for (int i = 0; i < _iterations; i++)
        {
            // Generate neighbor from current individual
            T mutated = _mutation.Mutate(individual.Individual);

            // Evaluate mutated individual
            var candidate = _fitnessEvaluator.GenerateEvaluated(mutated);

            // Accept only strictly better solutions
            if (_fitnessEvaluator.Compare(candidate, individual) < 0)
            {
                individual = candidate;
            }
        }

        return individual;
    }

    /// <summary>
    /// Applies hill climbing to a collection of non-evaluated individuals.
    /// </summary>
    public IReadOnlyList<EvaluatedIndividual<T>> Memetic(IReadOnlyList<T> individuals)
    {
        return Memetic(_fitnessEvaluator.GenerateEvaluated(individuals));
    }

    /// <summary>
    /// Applies hill climbing to a collection of evaluated individuals.
    /// </summary>
    public IReadOnlyList<EvaluatedIndividual<T>> Memetic(IReadOnlyList<EvaluatedIndividual<T>> individuals)
    {
        EvaluatedIndividual<T>[] result = new EvaluatedIndividual<T>[individuals.Count];

        Parallel.For(0, individuals.Count, i =>
        {
            result[i] = Memetic(individuals[i]);
        });

        return result;
    }
}