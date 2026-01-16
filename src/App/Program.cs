
class Program
{
    [STAThread]
    static void Main()
    {
        ProgramSetting? possibleSetting = FormProgram.Run();
        if (possibleSetting != null)
        {
            var setting = (ProgramSetting)possibleSetting;
            Console.WriteLine("Setting Loaded!");
            Console.WriteLine("Loading Packing Input...");
            var packingInput = PackingInputLoader.LoadFromFile(setting.IOSetting.SourceJson);
            Console.WriteLine("Packing Input Loaded!");
            Console.WriteLine("The Evolution Is Running...");
            var containers = EvolutionProgram.Run(setting, packingInput);
            PackingOutputSaver.SaveToFile(containers, setting.IOSetting.OutputJson);
        }
    }
}