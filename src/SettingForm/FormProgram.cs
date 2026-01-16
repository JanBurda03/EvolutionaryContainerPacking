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
                if (AlgorithmSettingsForms.TryGetValue(form.EvolutionSetting.AlgorithmName, out algorithmSettingForm))
                {
                    if (algorithmSettingForm.ShowDialog() == DialogResult.OK)
                    {
                        return new ProgramSetting(form.IOSetting, form.PackingSetting, form.EvolutionSetting, algorithmSettingForm.EvolutionaryAlgorithmSetting);
                    }                  
                    return null;
                    
                }
                else
                {
                    throw new Exception();
                }
            }
            return null;
            
        }
    }

    public static Dictionary<string, AlgorithmSettingForm> AlgorithmSettingsForms
    = new Dictionary<string, AlgorithmSettingForm>
    {
        { "Elitist Genetic", new ElitistGeneticSettingForm() },
        { "Hill Descending", new HillDescendingSettingForm() },
    };
}

