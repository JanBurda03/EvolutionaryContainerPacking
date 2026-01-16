public class EvaluatedIndividual<T>
{
    public T Individual { get; }
    public double Fitness { get; }

    public EvaluatedIndividual(T Individual, double Fitness)
    {
        this.Individual = Individual;
        this.Fitness = Fitness;
    }

    public static IReadOnlyList<EvaluatedIndividual<T>> MergeMultiple(IReadOnlyList<T> individuals, IReadOnlyList<double> fitnesses)
    {
        if (fitnesses.Count != individuals.Count)
        {
            throw new ArgumentException("List of individuals and list of fitnesses must have the same length");
        }


        EvaluatedIndividual<T>[] evaluated = new EvaluatedIndividual<T>[individuals.Count];

        for (int i = 0; i < individuals.Count; i++)
        {
            evaluated[i] = new EvaluatedIndividual<T>(individuals[i], fitnesses[i]);
        }

        return evaluated;
    }

    public static IReadOnlyList<double> GetFitnesses(IReadOnlyList<EvaluatedIndividual<T>> evaluated)
    {
        double[] fitnesses = new double[evaluated.Count];

        for (int i = 0; i < evaluated.Count; i++)
        {
            fitnesses[i] = evaluated[i].Fitness;
        }

        return fitnesses;
    }

    public static IReadOnlyList<T> GetIndividuals(IReadOnlyList<EvaluatedIndividual<T>> evaluated)
    {
        T[] individuals = new T[evaluated.Count];

        for (int i = 0;i< evaluated.Count; i++)
        {
            individuals[i] = evaluated[i].Individual;
        }

        return individuals;
    }
}
