namespace EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

/// <summary>
/// Interface for selecting best and worst individuals from a population.
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
    IReadOnlyList<EvaluatedIndividual<T>> GetElite(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites);

    /// <summary>
    /// Returns the best individual from the given population.
    /// </summary>
    /// <param name="population">The population to select elite from.</param>
    /// <returns>Best individual.</returns>
    EvaluatedIndividual<T> GetElite(IReadOnlyList<EvaluatedIndividual<T>> population);


    /// <summary>
    /// Splits the population into elite and non-elite individuals.
    /// </summary>
    /// <param name="population">The population to split.</param>
    /// <param name="numberOfElites">How many elites should be selected.</param>
    /// <returns>
    /// Tuple containing list of elite individuals and list of non-elite individuals.
    /// </returns>
    (IReadOnlyList<EvaluatedIndividual<T>> Elites, IReadOnlyList<EvaluatedIndividual<T>> NonElites) ElitesNonElitesSplit(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites);
}

