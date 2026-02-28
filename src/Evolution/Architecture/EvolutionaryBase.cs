namespace EvolutionaryContainerPacking.Evolution.Architecture;

using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Evolution.Architecture.Population;
using EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

/// <summary>
/// Base class for evolutionary algorithms.
/// Provides common population handling, best tracking and run logic.
/// </summary>
public abstract class EvolutionaryBase<T> : IEvolutionary<T>
{
    /// <summary>
    /// Current generation index.
    /// </summary>
    protected int CurrentGeneration { get; private set; }

    /// <summary>
    /// Current population.
    /// </summary>
    public IReadOnlyList<EvaluatedIndividual<T>> Population { get; protected set; }

    /// <summary>
    /// Best individual found so far.
    /// </summary>
    public EvaluatedIndividual<T> Best { get; private set; }

    private readonly double? targetFitness;

    private readonly IEvolutionStatistics<T> _evolutionStatistics;
    protected readonly IFitnessEvaluator<T> _fitnessEvaluator;
    protected readonly IPopulationFactory<T> _populationFactory;
    protected readonly IElitism<T> _elitism;

    private int _numberOfGenerationsPerRun;

    protected EvolutionaryBase(
        IPopulationFactory<T> populationFactory,
        IFitnessEvaluator<T> fitnessEvaluator,
        IElitism<T> elitism,
        EvolutionaryAlgorithmSetting setting,
        IEvolutionStatistics<T> evolutionStatistics)
    {
        _fitnessEvaluator = fitnessEvaluator;
        _populationFactory = populationFactory;
        _elitism = elitism;
        _evolutionStatistics = evolutionStatistics;
        targetFitness = setting.TargetFitness;
        _numberOfGenerationsPerRun = setting.NumberOfGenerations;

        IReadOnlyList<T> unevaluatedPopulation = _populationFactory.CreatePopulation(setting.NumberOfIndividuals);

        Population = fitnessEvaluator.GenerateEvaluated(unevaluatedPopulation);
        Best = _elitism.GetElite(Population);

        _evolutionStatistics.Update(CurrentGeneration, Population, Best);
    }

    /// <summary>
    /// Runs the algorithm for the configured number of generations.
    /// </summary>
    public T Run() => Run(_numberOfGenerationsPerRun);

    /// <summary>
    /// Runs the algorithm for a specified number of generations.
    /// </summary>
    public T Run(int numberOfGenerationsPerRun)
    {
        int finalNumberOfGenerations = CurrentGeneration + numberOfGenerationsPerRun;

        if (TargetFitnessAchieved())
        {
            return Best.Individual;
        }

        while (CurrentGeneration < finalNumberOfGenerations)
        {
            CurrentGeneration++;

            NextGeneration();

            var generationsBest = _elitism.GetElite(Population);

            if (_fitnessEvaluator.Compare(generationsBest, Best) < 0)
            {
                Best = generationsBest;
            }

            _evolutionStatistics.Update(
                CurrentGeneration,
                Population,
                generationsBest);

            if (TargetFitnessAchieved())
            {
                return Best.Individual;
            }
        }

        return Best.Individual;
    }

    /// <summary>
    /// Checks early stopping condition based on target fitness.
    /// </summary>
    private bool TargetFitnessAchieved()
    {
        if (targetFitness != null)
        {
            return _fitnessEvaluator.Compare(
                Best.Fitness,
                (double)targetFitness) < 0;
        }

        return false;
    }

    /// <summary>
    /// Produces the next generation (implemented by derived algorithms).
    /// </summary>
    protected abstract void NextGeneration();
}



