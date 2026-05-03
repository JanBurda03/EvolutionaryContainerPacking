namespace EvolutionaryContainerPacking.Tests.ElitismTests;

using EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Evolution.Architecture;

public class ElitismTests
{
    private static List<EvaluatedIndividual<string>> CreatePopulation()
        => new()
        {
            new EvaluatedIndividual<string>("A", 10),
            new EvaluatedIndividual<string>("B", 5),
            new EvaluatedIndividual<string>("C", 20),
            new EvaluatedIndividual<string>("D", 1),
            new EvaluatedIndividual<string>("E", 15),
            new EvaluatedIndividual<string>("F", 5),
            new EvaluatedIndividual<string>("G", 2)
        };

    [Fact]
    public void GetEliteMinimizing()
    {
        var elitism = new Elitism<string>(minimizing: true);
        var population = CreatePopulation();

        var elite = elitism.GetElite(population);

        Assert.Equal(1, elite.Fitness);
        Assert.Equal("D", elite.Individual);
    }

    [Fact]
    public void GetEliteMaximizing()
    {
        var elitism = new Elitism<string>(minimizing: false);
        var population = CreatePopulation();

        var elite = elitism.GetElite(population);

        Assert.Equal(20, elite.Fitness);
        Assert.Equal("C", elite.Individual);
    }


    [Fact]
    public void GetElites()
    {
        var elitism = new Elitism<string>(minimizing: true);
        var population = CreatePopulation();

        var elites = elitism.GetElite(population, 5);

        var fitnesses = elites.Select(e => e.Fitness).OrderBy(x => x).ToArray();

        Assert.Equal(new[] { 1.0, 2.0, 5.0, 5.0, 10.0}, fitnesses);
    }

    [Fact]
    public void GetEliteEmptyPopulationThrows()
    {
        var elitism = new Elitism<string>(true);
        var population = new List<EvaluatedIndividual<string>>();

        Assert.Throws<ArgumentException>(() => elitism.GetElite(population));
    }

    [Fact]
    public void GetEliteNullPopulationThrows()
    {
        var elitism = new Elitism<string>(true);

        Assert.Throws<ArgumentException>(() => elitism.GetElite(null));
    }

    [Fact]
    public void GetElitesCountZeroReturnsEmpty()
    {
        var elitism = new Elitism<string>(true);
        var population = CreatePopulation();

        var result = elitism.GetElite(population, 0);

        Assert.Empty(result);
    }

    [Fact]
    public void GetElitesNotEnoughIndividualsThrows()
    {
        var elitism = new Elitism<string>(true);
        var population = CreatePopulation();

        Assert.Throws<ArgumentException>(() => elitism.GetElite(population, 10));
    }
}