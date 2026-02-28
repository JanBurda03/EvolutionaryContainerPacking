namespace EvolutionaryContainerPacking.Evolution.Setting;

using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;
using EvolutionaryContainerPacking.Packing;

/// <summary>
/// Represents the complete high-level configuration of the application.
/// </summary>
/// <remarks>
/// This configuration defines:
/// <list type="bullet">
/// <item>The input packing problem (JSON file).</item>
/// <item>The output packing solution (JSON file).</item>
/// <item>Optional export of evolutionary statistics (CSV file).</item>
/// <item>The selected evolutionary algorithm and its parameters.</item>
/// <item>Packing-specific solver settings.</item>
/// </list>
/// </remarks>
public sealed record class ProgramSetting(

    /// <summary>
    /// Path to the input JSON file.
    /// The file must contain a serialized <see cref="PackingInput"/> instance.
    /// </summary>
    string SourceFile,

    /// <summary>
    /// Path to the output JSON file.
    /// The file will contain a serialized list of <see cref="ContainerData"/>
    /// representing the resulting packing solution.
    /// </summary>
    string OutputFile,

    /// <summary>
    /// Optional path to a CSV file where evolution statistics will be exported.
    /// If null, statistics export is skipped.
    /// </summary>
    string? EvolutionStatisticsFile,

    /// <summary>
    /// Name of the evolutionary algorithm to use.
    /// Typically used by a factory to select a specific algorithm implementation.
    /// </summary>
    string AlgorithmName,

    /// <summary>
    /// Configuration settings controlling packing constraints
    /// and solver behavior.
    /// </summary>
    PackingSetting PackingSetting,

    /// <summary>
    /// Configuration of the evolutionary algorithm.
    /// May be the base <see cref="EvolutionaryAlgorithmSettings"/>
    /// or a derived type containing algorithm-specific parameters.
    /// </summary>
    EvolutionaryAlgorithmSetting EvolutionAlgorithmSetting
);