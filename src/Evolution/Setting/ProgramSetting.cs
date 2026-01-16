
public readonly record struct ProgramSetting
{
    public ProgramSetting(IOSetting iOSetting, PackingSetting packingSetting, EvolutionSetting evolutionSetting, EvolutionaryAlgorithmSetting evolutionAlgorithmSetting)
    {
        IOSetting = iOSetting;
        PackingSetting = packingSetting;
        EvolutionSetting = evolutionSetting;
        EvolutionAlgorithmSetting = evolutionAlgorithmSetting;
    }

    public IOSetting IOSetting { get; init; }
    public PackingSetting PackingSetting { get; init; }

    public EvolutionSetting EvolutionSetting { get; init; }

    public EvolutionaryAlgorithmSetting EvolutionAlgorithmSetting { get; init; }

    
}

public readonly record struct IOSetting
{ 
    public IOSetting(string sourceJson, string outputJson)
    {
        SourceJson = sourceJson;
        OutputJson = outputJson;
    }
    public string SourceJson { get; init; }
    public string OutputJson { get; init; }
}

public readonly record struct EvolutionSetting
{
    public string AlgorithmName { get; init; }

    public int NumberOfIndividuals { get; init; }

    public int NumberOfGenerations { get; init; }



    public string IterationStatisticsFile { get; init; }


    public EvolutionSetting(string algorithmName, int numberOfIndividuals, int numberOfGenerations, string iterationStatisticsFile)
    {
        AlgorithmName = algorithmName;
        NumberOfIndividuals = numberOfIndividuals;
        NumberOfGenerations = numberOfGenerations;
        IterationStatisticsFile = iterationStatisticsFile;
    }
}