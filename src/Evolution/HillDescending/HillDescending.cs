public class HillDescending<T> : EvolutionaryBase<T>
{
    protected readonly IMutation<T> _mutator;

    protected readonly double _acceptanceDecay;

    protected double _acceptanceProbability;

    protected Random _random;

    protected double _endValue;

    public HillDescending(IReadOnlyList<T> initialPopulation, IFitnessEvaluator<T> fitnessEvaluator, IMutation<T> mutator, HillDescendingSetting setting, IIterationStatistics<T>? data = null) : base(initialPopulation, fitnessEvaluator, setting, data)
    {
        if (setting.AcceptanceDecay <= 0 || setting.AcceptanceDecay > 1)
            throw new ArgumentOutOfRangeException(nameof(setting.AcceptanceDecay),
                "AcceptanceDecay must be > 0 and <= 1.");

        _mutator = mutator;
        _acceptanceDecay = setting.AcceptanceDecay;
        _acceptanceProbability = setting.StartValue;
        _endValue = setting.EndValue;
        _random = new Random();
    }

    protected override void NextGeneration()
    {
        IReadOnlyList<T> mutated = _mutator.Mutate(Population);
        IReadOnlyList<EvaluatedIndividual<T>> evaluated = _fitnessEvaluator.GenerateEvaluated(mutated);

        EvaluatedIndividual<T>[] newPopulation = new EvaluatedIndividual<T>[Population.Count];

        for (int i = 0; i < newPopulation.Length; i++)
        {
            if (_fitnessEvaluator.Compare(evaluated[i], Population[i]) < 0 || _random.NextDouble() < _acceptanceProbability)
            {
                newPopulation[i] = evaluated[i];
            }
            else
            {
                newPopulation[i] = Population[i];
            }
        }

        Population = newPopulation;
        _acceptanceProbability = Math.Max(_acceptanceProbability * _acceptanceDecay, _endValue);
    }
}


