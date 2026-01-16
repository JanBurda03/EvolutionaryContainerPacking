public record class EvolutionaryAlgorithmSetting
{
    public EvolutionaryAlgorithmSetting(int? hardStop)
    {
        HardStop = hardStop;
    }
    public int? HardStop { get; init; }

}
