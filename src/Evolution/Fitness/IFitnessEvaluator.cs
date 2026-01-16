
public interface IFitnessEvaluator<T>
{
    public double EvaluateFitness(T t);

    public IReadOnlyList<double> EvaluateFitness(IReadOnlyList<T> t);

    public EvaluatedIndividual<T> GenerateEvaluated(T t);

    public IReadOnlyList<EvaluatedIndividual<T>> GenerateEvaluated(IReadOnlyList<T> t);

    public double Compare(T a, T b);

    public double Compare(double a, double b);

    public double Compare(EvaluatedIndividual<T> a, EvaluatedIndividual<T> b);

    

    public bool Minimizing {  get; }
}
