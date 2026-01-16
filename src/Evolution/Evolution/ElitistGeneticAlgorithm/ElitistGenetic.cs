using System.Text.Json;

public class ElitistGenetic<T> : EvolutionaryBase<T>
{

    protected readonly IMutation<T> _mutation;
    protected readonly ICrossover<T> _crossover;

    protected readonly IPopulationFactory<T> _factory;

    protected readonly ISelection<T> _randomSelection;

    protected readonly IElitism<T> _elitism;

    protected readonly int _numberOfElites;
    protected readonly int _numberOfNew;




    public ElitistGenetic(IReadOnlyList<T> initialPopulation, IFitnessEvaluator<T> fitnessEvaluator, ICrossover<T> crossover, IMutation<T> mutation, IPopulationFactory<T> factory, ElitistGeneticSetting setting, IIterationStatistics<T>? data = null): base(initialPopulation, fitnessEvaluator, setting, data)
    {
        _numberOfNew = setting.NumberOfNew;

        _numberOfElites = setting.NumberOfElitists;

        _crossover = crossover;

        _mutation = mutation;

        _elitism = new Elitism<T>(fitnessEvaluator.Minimizing);

        _randomSelection = new RandomSelection<T>();

        _factory = factory;


    }



    protected override void NextGeneration()
    {

        int numberOfSelected = Population.Count - _numberOfNew - _numberOfElites;

        var elites = _elitism.GetElites(Population, _numberOfElites);


        var selectedElites = _randomSelection.Select(elites, numberOfSelected);
        var selectedNonElites = _randomSelection.Select(Population, numberOfSelected);


        var crossovered = _crossover.Crossover(selectedNonElites, selectedElites);


        var mutated = _mutation.Mutate(crossovered);


        var added = _factory.CreatePopulation(_numberOfNew);


        var result = mutated.Concat(added).ToList();

        Population = (_fitnessEvaluator.GenerateEvaluated(result)).Concat(elites).ToList();
    }
}




