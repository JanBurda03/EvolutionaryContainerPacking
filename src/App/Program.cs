namespace EvolutionaryContainerPacking.App;

using EvolutionaryContainerPacking.Forms;
using EvolutionaryContainerPacking.Evolution.Setting;
using EvolutionaryContainerPacking.Evolution;
using EvolutionaryContainerPacking.App.Input;
using EvolutionaryContainerPacking.App.Output;

using System;

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
    static void Main(string[] args)
    {
        ProgramSetting? setting = null;

        // If a filename is provided as argument, load JSON config
        if (args.Length > 0)
        {
            try
            {
                string configFile = args[0];
                setting = ProgramSettingLoader.Load(configFile);
                Console.WriteLine($"Configuration loaded from '{configFile}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load configuration file: {ex.Message}");
                return;
            }
        }
        else
        {
            // Otherwise show WinForms settings dialog
            setting = FormProgram.Run();
        }

        if (setting != null)
        {
            try
            {
                // Load input data
                var packingInput = PackingInputLoader.Load(setting.SourceFile);

                // Run evolutionary algorithm
                (var containers, var statistics) = EvolutionProgram.Run(setting, packingInput);

                // Save packing result
                PackingOutputSaver.SaveToFile(containers, setting.OutputFile);

                // Optionally save evolution statistics
                if (setting.EvolutionStatisticsFile != null)
                {
                    EvolutionStatisticsSaver.SaveStatistics(statistics, setting.EvolutionStatisticsFile);
                }

                Console.WriteLine("Packing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during execution: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("No configuration provided; exiting.");
        }
    }
}