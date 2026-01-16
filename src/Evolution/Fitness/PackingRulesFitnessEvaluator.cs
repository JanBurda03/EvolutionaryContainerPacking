public class PackingRulesFitnessEvaluator:IFitnessEvaluator<PackingRules>
{
    // evaluating the fitness of packing rules firstly by finding a solution to it (filling the containers and returning list of them) and then evaluationg fitness of that solution
    private readonly IFitnessEvaluator<IReadOnlyList<ContainerData>> _containersFitnessEvaluator;
    private readonly PackingRulesSolver _packingRulesSolver;
    private FitnessComparer _fitnessComparer;

    public bool Minimizing { get; }

    public PackingRulesFitnessEvaluator(IFitnessEvaluator<IReadOnlyList<ContainerData>> containersFitnessEvaluator, PackingRulesSolver packingRulesSolver)
    {
        _containersFitnessEvaluator = containersFitnessEvaluator;
        _packingRulesSolver = packingRulesSolver;
        Minimizing = containersFitnessEvaluator.Minimizing;
        _fitnessComparer = new FitnessComparer(Minimizing);

    }

    public double EvaluateFitness(PackingRules packingRules)
    {
        IReadOnlyList<ContainerData> containers = _packingRulesSolver.Solve(packingRules);
        return _containersFitnessEvaluator.EvaluateFitness(containers);
    }

    public IReadOnlyList<double> EvaluateFitness(IReadOnlyList<PackingRules> packingRules)
    {
        double[] fitnesses = new double[packingRules.Count];

        Parallel.For(0, packingRules.Count, i =>
        {
            fitnesses[i] = EvaluateFitness(packingRules[i]);
        }); 

        return fitnesses;
    }

    public EvaluatedIndividual<PackingRules> GenerateEvaluated(PackingRules t)
    {
        double fitness = EvaluateFitness(t);
        return new EvaluatedIndividual<PackingRules>(t, fitness);
    }

    public IReadOnlyList<EvaluatedIndividual<PackingRules>> GenerateEvaluated(IReadOnlyList<PackingRules> t)
    {
        var fitnesses = EvaluateFitness(t);
        return EvaluatedIndividual<PackingRules>.MergeMultiple(t, fitnesses);
    }

    public static PackingRulesFitnessEvaluator Create(PackingInput inputData, PackingSetting packingSetting)
    {
        PackingRulesSolver packingVectorSolver = PackingProgram.CreateSolver(inputData, packingSetting);
        ContainersFitnessEvaluator containersFitnessEvaluator = new ContainersFitnessEvaluator();
        PackingRulesFitnessEvaluator packingVectorFintessEvaluator = new PackingRulesFitnessEvaluator(containersFitnessEvaluator, packingVectorSolver);
        return packingVectorFintessEvaluator;
    }

    public double Compare(PackingRules a, PackingRules b)
    {
        var aF = EvaluateFitness(a);
        var bF = EvaluateFitness(b);

        return Compare(aF, bF);
    }

    public double Compare(double a, double b)
    {
        return _fitnessComparer.Compare(a, b);
    }

    public double Compare(EvaluatedIndividual<PackingRules> a, EvaluatedIndividual<PackingRules> b)
    {
        return _fitnessComparer.Compare(a, b);
    }
}