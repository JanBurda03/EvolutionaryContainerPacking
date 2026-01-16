using System.Text.Json;

public static class PackingInputSaver
{
    public static void SaveToFile(PackingInput packingInput, string fileName)
    {

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(packingInput, options);
        File.WriteAllText(fileName, json);
    }
}
