namespace EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

/// <summary>
/// Interface for selecting elite individuals from a population.
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public interface IElitism<T>
{
    /// <summary>
    /// Returns the top-performing individuals from the given population.
    /// </summary>
    /// <param name="population">The population to select elites from.</param>
    /// <param name="numberOfElites">How many elites to return.</param>
    /// <returns>List of elite individuals.</returns>
    public IReadOnlyList<EvaluatedIndividual<T>> GetElites(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites);

    /// <summary>
    /// Returns the top-performing individual from the given population.
    /// </summary>
    /// <param name="population">The population to select elite from.</param>
    /// <returns>Elite individual.</returns>
    public EvaluatedIndividual<T> GetElite(IReadOnlyList<EvaluatedIndividual<T>> population);
}

