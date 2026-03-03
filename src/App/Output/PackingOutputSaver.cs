namespace EvolutionaryContainerPacking.App.Output;

using System.Text.Json;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;

/// <summary>
/// Provides functionality for saving packing results to a JSON file.
/// </summary>
public static class PackingOutputSaver
{
    /// <summary>
    /// Exports containers and saves them as formatted JSON.
    /// </summary>
    /// <param name="containers">Containers to export.</param>
    /// <param name="fileName">Output file path.</param>
    public static void SaveToFile(IReadOnlyList<ContainerData> containers, string fileName)
    {
        var exportContainers = containers.Select(container => container.ExportContainer()).ToList();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(exportContainers, options);
        File.WriteAllText(fileName, json);
    }
}