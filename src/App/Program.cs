namespace EvolutionaryContainerPacking.App;

using EvolutionaryContainerPacking.Forms;
using EvolutionaryContainerPacking.Evolution.EvolutionStatistics;
using EvolutionaryContainerPacking.Evolution.Setting;
using EvolutionaryContainerPacking.Packing.Rules;
using EvolutionaryContainerPacking.Evolution;
using System.Text;
using EvolutionaryContainerPacking.App.Input;
using EvolutionaryContainerPacking.App.Output;

class Program
{
    [STAThread]
    static void Main()
    {
        ProgramSetting? setting = FormProgram.Run();
        if (setting != null)
        {
            Console.WriteLine("Setting Loaded!");
            Console.WriteLine("Loading Packing Input...");
            var packingInput = PackingInputLoader.LoadFromFile(setting.SourceFile);
            Console.WriteLine("Packing Input Loaded!");
            Console.WriteLine("The Evolution Is Running...");
            (var containers, var statistics) = EvolutionProgram.Run(setting, packingInput);
            PackingOutputSaver.SaveToFile(containers, setting.OutputFile);

            if (setting.EvolutionStatisticsFile != null) 
            {
                SaveStatistics(statistics, setting.EvolutionStatisticsFile);
            }
        }
    }

    public static void SaveStatistics(IEvolutionStatistics<PackingRules> s, string fileName)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Generation;Best;Average");

        foreach (var i in s.EvolutionStatisticalData)
        {
            sb.AppendLine(
                $"{i.IterationNumber};{i.BestScore};{i.AverageScore}"
            );
        }

        File.WriteAllText(fileName, sb.ToString());
    }
}