namespace EvolutionaryContainerPacking.Evolution.Fitness;

using EvolutionaryContainerPacking.Packing.Architecture.Containers;
using EvolutionaryContainerPacking.Packing.Rules;
using EvolutionaryContainerPacking.Packing;
using EvolutionaryContainerPacking.Evolution.Architecture;

/// <summary>
/// Fitness evaluator for <see cref="PackingRules"/> individuals.
/// <para>
/// A packing rule is evaluated indirectly:
/// 1. The rule is decoded and solved using <see cref="PackingSolver"/>.
/// 2. The resulting container configuration is evaluated using a container fitness evaluator.
/// </para>
/// </summary>
public class PackingRulesFitnessEvaluator : IFitnessEvaluator<PackingRules>
{
    /// <summary>
    /// Evaluator used to assess the quality of the produced container configuration.
    /// </summary>
    private readonly IFitnessEvaluator<IReadOnlyList<ContainerData>> _containersFitnessEvaluator;

    /// <summary>
    /// Solver that transforms packing rules into an actual packing solution.
    /// </summary>
    private readonly PackingSolver _packingSolver;

    private readonly FitnessComparer _fitnessComparer;


    /// <summary>
    /// Indicates whether the optimization problem is minimizing or maximizing.
    /// Inherited from the container fitness evaluator.
    /// </summary>
    public bool Minimizing { get; }

    /// <summary>
    /// Initializes the evaluator using externally provided solver and container fitness evaluator.
    /// </summary>
    /// <param name="containersFitnessEvaluator">
    /// Evaluator used for container-level fitness computation.
    /// </param>
    /// <param name="packingSolver">
    /// Solver used to transform packing rules into container configurations.
    /// </param>

    
    
    

    /// <summary>
    /// Initializes the evaluator by constructing its own solver and container fitness evaluator.
    /// </summary>
    /// <param name="packingInput">Problem input data.</param>
    /// <param name="packingSetting">Packing configuration settings.</param>
    public PackingRulesFitnessEvaluator(PackingInput packingInput, PackingSetting packingSetting)
    {
        _packingSolver = new PackingSolver(packingInput, packingSetting);
        _containersFitnessEvaluator = new ContainersFitnessEvaluator();
        Minimizing = _containersFitnessEvaluator.Minimizing;
        _fitnessComparer = new FitnessComparer(Minimizing);
    }

    public PackingRulesFitnessEvaluator(
        IFitnessEvaluator<IReadOnlyList<ContainerData>> containersFitnessEvaluator,
        PackingSolver packingSolver)
    {
        _containersFitnessEvaluator = containersFitnessEvaluator;
        _packingSolver = packingSolver;
        Minimizing = containersFitnessEvaluator.Minimizing;
        _fitnessComparer = new FitnessComparer(Minimizing);
    }



    /// <summary>
    /// Evaluates a single packing rule by solving it and computing
    /// the fitness of the resulting container configuration.
    /// </summary>
    /// <param name="packingRules">Packing rule to evaluate.</param>
    /// <returns>Fitness value (lower is better for minimization problems).</returns>
    public double EvaluateFitness(PackingRules packingRules)
    {
        IReadOnlyList<ContainerData> containers = _packingSolver.Solve(packingRules);
        return _containersFitnessEvaluator.EvaluateFitness(containers);
    }

    /// <summary>
    /// Evaluates multiple packing rules in parallel.
    /// </summary>
    /// <param name="packingRules">Collection of packing rules.</param>
    /// <returns>List of fitness values.</returns>
    public IReadOnlyList<double> EvaluateFitness(IReadOnlyList<PackingRules> packingRules)
    {
        double[] fitnesses = new double[packingRules.Count];

        Parallel.For(0, packingRules.Count, i =>
        {
            fitnesses[i] = EvaluateFitness(packingRules[i]);
        });

        return fitnesses;
    }

    /// <summary>
    /// Evaluates a packing rule and wraps it together with its fitness value.
    /// </summary>
    /// <param name="t">Packing rule to evaluate.</param>
    /// <returns>Evaluated individual.</returns>
    public EvaluatedIndividual<PackingRules> GenerateEvaluated(PackingRules t)
    {
        double fitness = EvaluateFitness(t);
        return new EvaluatedIndividual<PackingRules>(t, fitness);
    }

    /// <summary>
    /// Evaluates multiple packing rules and wraps them with their fitness values.
    /// </summary>
    /// <param name="t">Collection of packing rules.</param>
    /// <returns>List of evaluated individuals.</returns>
    public IReadOnlyList<EvaluatedIndividual<PackingRules>> GenerateEvaluated(IReadOnlyList<PackingRules> t)
    {
        var fitnesses = EvaluateFitness(t);
        return EvaluatedIndividual<PackingRules>.Merge(t, fitnesses);
    }

    /// <summary>
    /// Compares two packing rules based on their evaluated fitness.
    /// </summary>
    public int Compare(PackingRules a, PackingRules b)
    {
        var aF = EvaluateFitness(a);
        var bF = EvaluateFitness(b);
        return Compare(aF, bF);
    }

    /// <summary>
    /// Compares two raw fitness values.
    /// </summary>
    public int Compare(double a, double b)
    {
        return _fitnessComparer.Compare(a, b);
    }

    /// <summary>
    /// Compares two evaluated packing rules.
    /// </summary>
    public int Compare(EvaluatedIndividual<PackingRules> a, EvaluatedIndividual<PackingRules> b)
    {
        return _fitnessComparer.Compare(a, b);
    }
}