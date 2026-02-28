namespace EvolutionaryContainerPacking.Evolution.Fitness;

using EvolutionaryContainerPacking.Packing.Architecture.Containers;
using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Fitness evaluator for container packing solutions.
/// Evaluates a solution based on the number of containers used
/// and the utilization of the least filled container.
/// </summary>
/// <remarks>
/// The objective is minimizing:
/// - primarily the number of containers,
/// - secondarily the worst (minimum) container utilization.
/// </remarks>
public class ContainersFitnessEvaluator : IFitnessEvaluator<IReadOnlyList<ContainerData>>
{
    /// <summary>
    /// Indicates that this is a minimization problem.
    /// Lower fitness values represent better solutions.
    /// </summary>
    public bool Minimizing { get; } = true;

    private FitnessComparer _fitnessComparer;

    /// <summary>
    /// Initializes the fitness evaluator.
    /// </summary>
    public ContainersFitnessEvaluator()
    {
        _fitnessComparer = new FitnessComparer(Minimizing);
    }

    /// <summary>
    /// Computes the fitness of a packing solution.
    /// <para>
    /// Fitness is defined as:
    /// <code>
    /// number_of_containers + minimum_container_utilization
    /// </code>
    /// where container utilization is the maximum of relative weight
    /// and relative volume usage.
    /// </para>
    /// </summary>
    /// <param name="containers">Packed containers representing a solution.</param>
    /// <returns>Fitness value (lower is better).</returns>
    public double EvaluateFitness(IReadOnlyList<ContainerData> containers)
    {
        double currentMin = double.MaxValue;

        foreach (var container in containers)
        {
            double value = Math.Max(container.GetRelativeWeight(), container.GetRelativeVolume());
            if (value < currentMin)
            {
                currentMin = value;
            }
        }

        return currentMin + containers.Count;
    }

    /// <summary>
    /// Computes fitness values for multiple packing solutions.
    /// </summary>
    /// <param name="t">Collection of packing solutions.</param>
    /// <returns>List of fitness values.</returns>
    public IReadOnlyList<double> EvaluateFitness(IReadOnlyList<IReadOnlyList<ContainerData>> t)
    {
        double[] fitnesses = new double[t.Count];

        for (int i = 0; i < fitnesses.Length; i++)
        {
            fitnesses[i] = EvaluateFitness(t[i]);
        }

        return fitnesses;
    }

    /// <summary>
    /// Evaluates a single packing solution and wraps it together with its fitness value.
    /// </summary>
    /// <param name="containers">Packing solution.</param>
    /// <returns>Evaluated individual containing solution and fitness.</returns>
    public EvaluatedIndividual<IReadOnlyList<ContainerData>> GenerateEvaluated(IReadOnlyList<ContainerData> containers)
    {
        double fitness = EvaluateFitness(containers);
        return new EvaluatedIndividual<IReadOnlyList<ContainerData>>(containers, fitness);
    }

    /// <summary>
    /// Evaluates multiple packing solutions and wraps them with their fitness values.
    /// </summary>
    /// <param name="t">Collection of packing solutions.</param>
    /// <returns>List of evaluated individuals.</returns>
    public IReadOnlyList<EvaluatedIndividual<IReadOnlyList<ContainerData>>> GenerateEvaluated(IReadOnlyList<IReadOnlyList<ContainerData>> t)
    {
        IReadOnlyList<double> fitnesses = EvaluateFitness(t);
        return EvaluatedIndividual<IReadOnlyList<ContainerData>>.Merge(t, fitnesses);
    }

    /// <summary>
    /// Compares two packing solutions based on their fitness values.
    /// </summary>
    /// <param name="a">First solution.</param>
    /// <param name="b">Second solution.</param>
    /// <returns>
    /// Comparison result indicating which solution is better
    /// according to the minimization objective.
    /// </returns>
    public int Compare(IReadOnlyList<ContainerData> a, IReadOnlyList<ContainerData> b)
    {
        var aF = EvaluateFitness(a);
        var bF = EvaluateFitness(b);
        return Compare(aF, bF);
    }

    /// <summary>
    /// Compares two raw fitness values.
    /// </summary>
    /// <param name="a">First fitness value.</param>
    /// <param name="b">Second fitness value.</param>
    /// <returns>
    /// Comparison result indicating which value is better
    /// according to the minimization objective.
    /// </returns>
    public int Compare(double a, double b)
    {
        return _fitnessComparer.Compare(a, b);
    }

    /// <summary>
    /// Compares two evaluated packing solutions.
    /// </summary>
    /// <param name="a">First evaluated solution.</param>
    /// <param name="b">Second evaluated solution.</param>
    /// <returns>
    /// Comparison result indicating which evaluated solution is better.
    /// </returns>
    public int Compare(EvaluatedIndividual<IReadOnlyList<ContainerData>> a, EvaluatedIndividual<IReadOnlyList<ContainerData>> b)
    {
        return _fitnessComparer.Compare(a, b);
    }
}