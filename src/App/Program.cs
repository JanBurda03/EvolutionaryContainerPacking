namespace EvolutionaryContainerPacking.App;

using EvolutionaryContainerPacking.Forms;
using EvolutionaryContainerPacking.Evolution.Setting;
using EvolutionaryContainerPacking.Evolution;
using EvolutionaryContainerPacking.App.Input;
using EvolutionaryContainerPacking.App.Output;

/// <summary>
/// Application entry point.
/// Handles user configuration, runs the evolutionary packing algorithm,
/// and saves results to files.
/// </summary>
class Program
{
    /// <summary>
    /// Main application method (STA required for WinForms).
    /// </summary>
    [STAThread]
    static void Main()
    {
        ProgramSetting? setting = FormProgram.Run();

        if (setting != null)
        {
            // Load input data
            var packingInput = PackingInputLoader.LoadFromFile(setting.SourceFile);

            // Run evolutionary algorithm
            (var containers, var statistics) = EvolutionProgram.Run(setting, packingInput);

            // Save packing result
            PackingOutputSaver.SaveToFile(containers, setting.OutputFile);

            // Optionally save evolution statistics
            if (setting.EvolutionStatisticsFile != null)
            {
                EvolutionStatisticsSaver.SaveStatistics(statistics, setting.EvolutionStatisticsFile);
            }
        }
    }
}