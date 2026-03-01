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
    /// Returns the worst-performing individuals from the given population.
    /// </summary>
    /// <param name="population">The population to select worst individuals from.</param>
    /// <param name="numberOfWorst">How many worst individuals to return.</param>
    /// <returns>List of worst individuals.</returns>
    IReadOnlyList<EvaluatedIndividual<T>> GetWorst(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfWorst);

    /// <summary>
    /// Returns the worst individual from the given population.
    /// </summary>
    /// <param name="population">The population to select worst from.</param>
    /// <returns>Worst individual.</returns>
    EvaluatedIndividual<T> GetWorst(IReadOnlyList<EvaluatedIndividual<T>> population);
}

