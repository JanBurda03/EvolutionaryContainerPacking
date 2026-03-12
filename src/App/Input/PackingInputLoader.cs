namespace EvolutionaryContainerPacking.App.Input;

using EvolutionaryContainerPacking.Packing;
using System.Text.Json;

/// <summary>
/// Loads packing input data from a JSON file.
/// </summary>
public static class PackingInputLoader
{
    /// <summary>
    /// Reads and deserializes <see cref="PackingInput"/> from the specified file.
    /// </summary>
    /// <param name="fileName">Path to the JSON file.</param>
    /// <returns>Deserialized packing input.</returns>
    /// <exception cref="Exception">Thrown if deserialization fails.</exception>
    public static PackingInput Load(string fileName)
    {
        string json = File.ReadAllText(fileName);
        PackingInput? input = JsonSerializer.Deserialize<PackingInput>(json);

        if (input == null)
            throw new Exception("JSON deserialize error!");

        return input;
    }
}


