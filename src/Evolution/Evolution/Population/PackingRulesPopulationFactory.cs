public class PackingRulesPopulationFactory : IPopulationFactory <PackingRules>
{
    private readonly int _packingVectorLength;

    public PackingRulesPopulationFactory(int packingVectorLength)
    {
        _packingVectorLength = packingVectorLength;
    }

    public IReadOnlyList<PackingRules> CreatePopulation(int numberOfIndividuals)
    {
        PackingRules[] population = new PackingRules[numberOfIndividuals];

        for(int i = 0; i < population.Length; i++) 
        {
            population[i] = PackingRules.CreateRandom(_packingVectorLength);
        }

        return Array.AsReadOnly(population);
    }


}
