public interface IPopulationFactory<T>
{
    public IReadOnlyList<T> CreatePopulation(int numberOfIndividuals);
}