namespace EvolutionaryContainerPacking.Evolution.Architecture.ProbabilisticHillClimbing;

using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Architecture.Population;
using EvolutionaryContainerPacking.Evolution.Architecture.Mutation;
using EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

/// <summary>
/// Population-based probabilistic hill climbing algorithm.
/// Worse solutions may be accepted with a gradually decreasing probability.
/// </summary>
public class ProbabilisticHillClimbing<T> : EvolutionaryBase<T>
{
    protected readonly IMutation<T> _mutator;
    protected readonly double _acceptanceDecayFactor;

    protected double _acceptanceProbability;
    protected double _minimalAcceptanceProbability;

    protected Random _random;

    public ProbabilisticHillClimbing(
        IPopulationFactory<T> populationFactory,
        IFitnessEvaluator<T> fitnessEvaluator,
        IElitism<T> elitism,
        IMutation<T> mutator,
        ProbabilisticHillClimbingSetting setting,
        IEvolutionStatistics<T> evolutionStatistics)

        : base(populationFactory, fitnessEvaluator, elitism, setting, evolutionStatistics)
    {
        if (setting.AcceptanceDecayFactor <= 0 || setting.AcceptanceDecayFactor > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(setting.AcceptanceDecayFactor), "Must be > 0 and <= 1.");
        }

        _mutator = mutator;
        _acceptanceDecayFactor = setting.AcceptanceDecayFactor;
        _acceptanceProbability = setting.StartAcceptanceProbability;
        _minimalAcceptanceProbability = setting.EndAcceptanceProbability;
        _random = new Random();
    }

    /// <summary>
    /// Generates the next generation using mutation
    /// and probabilistic acceptance of worse solutions.
    /// </summary>
    protected override void NextGeneration()
    {
        // Mutate current population
        IReadOnlyList<T> mutated = _mutator.Mutate(Population);

        // Evaluate mutated individuals
        IReadOnlyList<EvaluatedIndividual<T>> evaluated = _fitnessEvaluator.GenerateEvaluated(mutated);

        EvaluatedIndividual<T>[] newPopulation = new EvaluatedIndividual<T>[Population.Count];

        for (int i = 0; i < newPopulation.Length; i++)
        {
            // Accept if better, or with certain probability if worse
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

        // Decrease acceptance probability (annealing effect)
        _acceptanceProbability = Math.Max(_acceptanceProbability * _acceptanceDecayFactor, _minimalAcceptanceProbability);
    }
}


