namespace EvolutionaryContainerPacking.Evolution.Architecture.Population;

/// <summary>
/// Defines a factory for creating initial populations.
/// </summary>
/// <typeparam name="T">
/// Type of the individual representation.
/// </typeparam>
public interface IPopulationFactory<T>
{
    /// <summary>
    /// Creates a population with the specified number of individuals.
    /// </summary>
    IReadOnlyList<T> CreatePopulation(int numberOfIndividuals);
}