namespace EvolutionaryContainerPacking.Evolution.EvolutionStatistics;

/// <summary>
/// Represents statistical data for a single iteration
/// of the evolutionary process.
/// </summary>

public readonly record struct StatisticalData(
    int IterationNumber,
    double BestScore,
    double AverageScore
);