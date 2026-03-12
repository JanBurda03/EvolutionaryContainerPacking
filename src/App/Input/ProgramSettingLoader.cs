namespace EvolutionaryContainerPacking.App.Input;

using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Evolution.Setting;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Provides functionality for loading the application configuration
/// from a JSON file.
/// </summary>
/// <remarks>
/// This loader configures JSON polymorphic deserialization for
/// <see cref="EvolutionaryAlgorithmSetting"/> so that specific algorithm
/// configuration types can be instantiated automatically based on
/// the <c>algorithmName</c> discriminator property in the JSON configuration.
/// </remarks>
public static class ProgramSettingLoader
{
    /// <summary>
    /// JSON serializer options used for configuration loading.
    /// </summary>
    /// <remarks>
    /// Includes polymorphic mapping for evolutionary algorithm settings
    /// and enables case-insensitive property matching.
    /// </remarks>
    private static readonly JsonSerializerOptions Options = CreateSerializerOptions();

    /// <summary>
    /// Creates configured serializer options including polymorphic
    /// deserialization rules for evolutionary algorithm settings.
    /// </summary>
    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var resolver = new DefaultJsonTypeInfoResolver();

        resolver.Modifiers.Add(typeInfo =>
        {
            if (typeInfo.Type == typeof(EvolutionaryAlgorithmSetting))
            {
                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    /// <summary>
                    /// JSON property used to determine the concrete algorithm type.
                    /// </summary>
                    TypeDiscriminatorPropertyName = "AlgorithmName",

                    /// <summary>
                    /// Unknown types are not allowed and will produce an exception.
                    /// </summary>
                    IgnoreUnrecognizedTypeDiscriminators = false
                };

                /// <summary>
                /// Elitist Genetic Algorithm configuration.
                /// </summary>
                typeInfo.PolymorphismOptions.DerivedTypes.Add(
                    new JsonDerivedType(
                        typeof(ElitistGeneticAlgorithmSetting),
                        "Elitist Genetic"));

                /// <summary>
                /// Probabilistic Hill Climbing configuration.
                /// </summary>
                typeInfo.PolymorphismOptions.DerivedTypes.Add(
                    new JsonDerivedType(
                        typeof(ProbabilisticHillClimbingSetting),
                        "Hill Climbing"));
            }
        });

        return new JsonSerializerOptions
        {
            /// <summary>
            /// Allows case-insensitive property matching when reading JSON.
            /// </summary>
            PropertyNameCaseInsensitive = true,

            /// <summary>
            /// Enables the configured polymorphic resolver.
            /// </summary>
            TypeInfoResolver = resolver
        };
    }

    /// <summary>
    /// Loads the program configuration from a JSON file.
    /// </summary>
    /// <param name="path">
    /// Path to the configuration file.
    /// </param>
    /// <returns>
    /// Deserialized <see cref="ProgramSetting"/> instance.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the configuration file does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the configuration cannot be deserialized.
    /// </exception>
    public static ProgramSetting Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Configuration file not found: {path}");
        }

        string json = File.ReadAllText(path);

        ProgramSetting? setting =
            JsonSerializer.Deserialize<ProgramSetting>(json, Options);

        return setting
            ?? throw new InvalidOperationException(
                "Failed to deserialize program configuration.");
    }
}