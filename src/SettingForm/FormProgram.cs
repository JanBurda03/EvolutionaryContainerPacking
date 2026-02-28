namespace EvolutionaryContainerPacking.Forms;

using EvolutionaryContainerPacking.Evolution.Setting;

public static class FormProgram
{
    [STAThread]
    public static void Main() { Run(); }

    public static ProgramSetting? Run()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using (var form = new SettingsForm())
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                AlgorithmSettingForm algorithmSettingForm;
                if (AlgorithmSettingsForms.TryGetValue(form.AlgorithmName, out algorithmSettingForm))
                {
                    if (algorithmSettingForm.ShowDialog() == DialogResult.OK)
                    {
                        return new ProgramSetting(form.SourceFile, form.OutputFile, form.EvolutionStatisticsFile, form.AlgorithmName, form.PackingSetting,  algorithmSettingForm.EvolutionaryAlgorithmSetting);
                    }                  
                    return null;
                    
                }
                else
                {
                    throw new Exception("No algorithm with given name found!");
                }
            }
            return null;
            
        }
    }

    public static Dictionary<string, AlgorithmSettingForm> AlgorithmSettingsForms
    = new Dictionary<string, AlgorithmSettingForm>
    {
        { "Elitist Genetic", new ElitistGeneticSettingForm() },
        { "Hill Climbing", new ProbabilisticHillClimbingSettingForm() },
    };
}

