namespace EvolutionaryContainerPacking.Forms;

using EvolutionaryContainerPacking.Evolution.Setting;

/// <summary>
/// Handles application startup through WinForms dialogs
/// and collects user-defined program settings.
/// </summary>
public static class FormProgram
{
    /// <summary>
    /// Displays configuration dialogs and returns selected program settings.
    /// </summary>
    /// <returns>
    /// Configured <see cref="ProgramSetting"/> or null if the user cancels.
    /// </returns>
    public static ProgramSetting? Run()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using (var form = new SettingsForm())
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (AlgorithmSettingsForms.TryGetValue(form.AlgorithmName, out AlgorithmSettingForm algorithmSettingForm))
                {
                    if (algorithmSettingForm.ShowDialog() == DialogResult.OK)
                    {
                        return new ProgramSetting(
                            form.SourceFile,
                            form.OutputFile,
                            form.EvolutionStatisticsFile,
                            form.AlgorithmName,
                            form.PackingSetting,
                            algorithmSettingForm.EvolutionaryAlgorithmSetting);
                    }

                    return null;
                }

                throw new Exception("No algorithm with given name found!");
            }

            return null;
        }
    }

    /// <summary>
    /// Available algorithm configuration forms mapped by algorithm name.
    /// </summary>
    public static Dictionary<string, AlgorithmSettingForm> AlgorithmSettingsForms
        = new Dictionary<string, AlgorithmSettingForm>
        {
            { "Elitist Genetic", new ElitistGeneticSettingForm() },
            { "Hill Climbing", new ProbabilisticHillClimbingSettingForm() },
        };
}

