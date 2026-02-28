namespace EvolutionaryContainerPacking.App.Output;

using System.Text.Json;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;

public static class PackingOutputSaver
{
    public static void SaveToFile(IReadOnlyList<ContainerData> containers, string fileName)
    {
        var exportContainers = containers
            .Select(container => container.ExportContainer())
            .ToList();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(exportContainers, options);
        File.WriteAllText(fileName, json);
    }
}