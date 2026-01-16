
public abstract class EvolutionaryBase<T> : IEvolutionary<T>
{

    public int CurrentGeneration { get; protected set; }
    public IReadOnlyList<EvaluatedIndividual<T>> Population { get; protected set; }
    public EvaluatedIndividual<T> CurrentGenerationBest { get; protected set; }
    public EvaluatedIndividual<T> GlobalBest { get; protected set; }

    protected readonly double? _hardStop;

    protected readonly IIterationStatistics<T> _statistics;

    protected readonly IFitnessEvaluator<T> _fitnessEvaluator;


    public EvolutionaryBase(IReadOnlyList<T> initialPopulation, IFitnessEvaluator<T> fitnessEvaluator, EvolutionaryAlgorithmSetting setting, IIterationStatistics<T>? statistics = null)
    {
        _fitnessEvaluator = fitnessEvaluator;
        Population = fitnessEvaluator.GenerateEvaluated(initialPopulation);

        CurrentGeneration = 0;

        // class for statistical purposes (for example getting data for purpose of graphical representation of the best individual in every generation)
        _statistics = statistics ?? new EmptyStatistics<T>();
        _hardStop = setting.HardStop;


        // evaluation of generation 0

        GlobalBest = FindBestIndividualInGeneration();
        CurrentGenerationBest = GlobalBest;


    }

    public void Evolve(int numberOfGenerations)
    {

        for (int i = 0; i < numberOfGenerations; i++)
        {
            CurrentGeneration++;

            NextGeneration();

            CurrentGenerationBest = FindBestIndividualInGeneration();

            if (_fitnessEvaluator.Compare(CurrentGenerationBest, GlobalBest) < 0)
            {
                GlobalBest = CurrentGenerationBest;
            }

            _statistics.Update(CurrentGeneration, Population, CurrentGenerationBest);

            if (HardStopNow())
            {
                return;
            }
        }
    }

    protected bool HardStopNow()
    {
        if (_hardStop == null)
        {
            return false;
        }

        // checking whether the limit for fitness has been reached

        return (_fitnessEvaluator.Compare(GlobalBest.Fitness, (double)_hardStop) < 0);

    }

    protected EvaluatedIndividual<T> FindBestIndividualInGeneration()
    {
        var individual = Population[0];

        for (int i = 1; i < Population.Count; i++)
        {
            if (_fitnessEvaluator.Compare(Population[i], individual) < 0)
            {
                individual = Population[i];
            }
        }
        return individual;
    }

    protected abstract void NextGeneration();


}



