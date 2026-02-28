namespace EvolutionaryContainerPacking.Evolution.Architecture.Population;

using EvolutionaryContainerPacking.Packing.Rules;

/// <summary>
/// Creates initial populations of random packing rules.
/// </summary>
public class PackingRulesPopulationFactory: IPopulationFactory<PackingRules>
{
    private readonly int _packingVectorLength;

    public PackingRulesPopulationFactory(int packingVectorLength)
    {
        _packingVectorLength = packingVectorLength;
    }

    /// <summary>
    /// Creates a population of randomly generated packing rules.
    /// </summary>
    public IReadOnlyList<PackingRules> CreatePopulation(int numberOfIndividuals)
    {
        PackingRules[] population = new PackingRules[numberOfIndividuals];

        for (int i = 0; i < population.Length; i++)
        {
            population[i] = PackingRules.CreateRandom(_packingVectorLength);
        }

        return Array.AsReadOnly(population);
    }
}