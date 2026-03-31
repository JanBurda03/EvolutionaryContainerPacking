namespace EvolutionaryContainerPacking.Evolution;

using EvolutionaryContainerPacking.Packing.Architecture.Containers;
using EvolutionaryContainerPacking.Packing;
using EvolutionaryContainerPacking.Packing.Rules;
using EvolutionaryContainerPacking.Evolution.Fitness;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Setting;
using EvolutionaryContainerPacking.Evolution.Architecture.Population;
using EvolutionaryContainerPacking.Evolution.Setting.EvolutionaryAlgorithmSettings;

/// <summary>
/// Entry point for executing the evolutionary container packing process.
/// </summary>
/// <remarks>
/// This class orchestrates the complete workflow:
/// <list type="number">
/// <item>Create fitness evaluator.</item>
/// <item>Create population factory.</item>
/// <item>Instantiate the selected evolutionary algorithm.</item>
/// <item>Run evolution to obtain the best individual.</item>
/// <item>Use the best evolved packing rules to compute the final packing solution.</item>
/// </list>
/// </remarks>
public static class EvolutionProgram
{
    /// <summary>
    /// Executes the selected evolutionary algorithm and returns the final packing solution
    /// together with evolution statistics.
    /// </summary>
    /// <param name="setting">
    /// High-level program configuration including algorithm selection, solver setting, input/ouput information
    /// and evolutionary parameters.
    /// </param>
    /// <param name="packingInput">
    /// Input data describing containers and boxes to be packed.
    /// </param>
    /// <returns>
    /// A tuple containing:
    /// <list type="bullet">
    /// <item>
    /// The computed packing solution (containers with placed boxes).
    /// </item>
    /// <item>
    /// Evolution statistics collected during the run.
    /// </item>
    /// </list>
    /// </returns>
    public static (IReadOnlyList<ContainerData> solution, IEvolutionStatistics<PackingRules> statistics) Run(ProgramSetting setting, PackingInput packingInput)
    {
        // Converts relative population size in the evolutionary algorithm setting to absolute
        setting = ConvertRelativeSettingToAbsolute(setting, packingInput);

        // Create fitness evaluator responsible for scoring individuals
        var fitnessEvaluator = new PackingRulesFitnessEvaluator(packingInput, setting.PackingSetting);

        // Initialize statistics collector
        IEvolutionStatistics<PackingRules> evolutionStatistics = 
            (setting.EvolutionStatisticsFile != null) ? 
            new EvolutionStatistics<PackingRules>() : 
            new ConsoleOnlyEvolutionStatistics<PackingRules>();

        // Create factory responsible for generating populations
        var populationFactory = new PackingRulesPopulationFactory(PackingSolver.GetPackingRulesMinimalLength(packingInput, setting.PackingSetting));

        // Instantiate selected evolutionary algorithm
        var evolutionaryAlgorithm = EvolutionaryAlgorithms.GetEvolutionaryAlgorithm(
            populationFactory,
            fitnessEvaluator,
            setting.EvolutionAlgorithmSetting,
            evolutionStatistics);

        // Execute evolutionary process and obtain best evolved individual (packing rules)
        var bestIndividual = evolutionaryAlgorithm.Run();

        // Use best evolved packing rules to construct final packing solution
        var solver = new PackingSolver(packingInput, setting.PackingSetting);
        var solution = solver.Solve(bestIndividual);

        return (solution, evolutionStatistics);
    }

    /// <summary>
    /// Converts relative population size in the evolutionary algorithm setting
    /// to an absolute value based on the provided packing input.
    /// </summary>
    private static ProgramSetting ConvertRelativeSettingToAbsolute(
        ProgramSetting setting,
        PackingInput packingInput)
    {
        EvolutionaryAlgorithmSetting evolutionAlgorithmSetting = setting.EvolutionAlgorithmSetting;

        // If already absolute, no conversion is needed
        if (!evolutionAlgorithmSetting.UseIndividualsAsRelative)
        {
            return setting;
        }

        // Compute absolute population size based on problem size (number of boxes)
        int absoluteIndividuals = evolutionAlgorithmSetting.Individuals * packingInput.BoxPropertiesList.Count;

        // Create new algorithm setting with resolved (absolute) population size
        EvolutionaryAlgorithmSetting absoluteEvolutionAlgorithmSetting =
            evolutionAlgorithmSetting with
            {
                UseIndividualsAsRelative = false,
                Individuals = absoluteIndividuals,
            };

        // Return updated program setting with the resolved algorithm configuration
        return setting with
        {
            EvolutionAlgorithmSetting = absoluteEvolutionAlgorithmSetting,
        };
    }
}

