namespace EvolutionaryContainerPacking.App.Output;

using System.Text;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Packing.Rules;

/// <summary>
/// Provides functionality for saving evolution statistics to a file.
/// </summary>
public static class EvolutionStatisticsSaver
{
    /// <summary>
    /// Saves evolution statistics into a CSV file (Generation;Best;Average).
    /// </summary>
    /// <param name="statistics">Evolution statistics instance.</param>
    /// <param name="fileName">Output file path.</param>
    public static void SaveStatistics(IEvolutionStatistics<PackingRules> statistics, string fileName)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Generation;Best;Average");

        foreach (var i in statistics.EvolutionStatisticalData)
        {
            sb.AppendLine($"{i.IterationNumber};{i.BestScore};{i.AverageScore}");
        }

        File.WriteAllText(fileName, sb.ToString());
    }
}