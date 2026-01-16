public class ContainersFitnessEvaluator : IFitnessEvaluator<IReadOnlyList<ContainerData>>
{
    public bool Minimizing { get; } = true;

    private FitnessComparer _fitnessComparer;

    public ContainersFitnessEvaluator()
    {
        _fitnessComparer = new FitnessComparer(Minimizing);
    }

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

    public IReadOnlyList<double> EvaluateFitness(IReadOnlyList<IReadOnlyList<ContainerData>> t)
    {
        double[] fitnesses = new double[t.Count];

        for (int i = 0; i < fitnesses.Length; i++)
        {
            fitnesses[i] = EvaluateFitness(t[i]);
        }
        return fitnesses;
    }

    public EvaluatedIndividual<IReadOnlyList<ContainerData>> GenerateEvaluated(IReadOnlyList<ContainerData> containers)
    {
        double fitness = EvaluateFitness(containers);
        return new EvaluatedIndividual<IReadOnlyList<ContainerData>>(containers, fitness);
    }

    public IReadOnlyList<EvaluatedIndividual<IReadOnlyList<ContainerData>>> GenerateEvaluated(IReadOnlyList<IReadOnlyList<ContainerData>> t)
    {
        IReadOnlyList<double> fitnesses = EvaluateFitness(t);
        return EvaluatedIndividual<IReadOnlyList<ContainerData>>.MergeMultiple(t, fitnesses);
    }



    public double Compare(IReadOnlyList<ContainerData> a, IReadOnlyList<ContainerData> b)
    {
        var aF = EvaluateFitness(a);
        var bF = EvaluateFitness(b);
        return Compare(aF, bF);
    }

    public double Compare(double a, double b)
    {
        return _fitnessComparer.Compare(a, b);
    }

    public double Compare(EvaluatedIndividual<IReadOnlyList<ContainerData>> a, EvaluatedIndividual<IReadOnlyList<ContainerData>> b)
    {
        return _fitnessComparer.Compare(a, b);
    }
}