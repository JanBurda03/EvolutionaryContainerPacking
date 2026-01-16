public class FitnessComparer
{
    private readonly bool _minimizing;

    public FitnessComparer(bool minimizing)
    {
        _minimizing = minimizing;
    }

    public int Compare(double x, double y)
    {
        int comparison = x.CompareTo(y);
        return _minimizing ? comparison : -comparison;
    }

    public int Compare<T>(EvaluatedIndividual<T> x, EvaluatedIndividual<T> y)
    {
        int comparison = x.Fitness.CompareTo(y.Fitness);
        return _minimizing ? comparison : -comparison;
    }
}