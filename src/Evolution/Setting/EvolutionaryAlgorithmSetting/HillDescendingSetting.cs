public record HillDescendingSetting : EvolutionaryAlgorithmSetting
{
    public double AverageElementsChanged { get; init; }
    public double AcceptanceDecay { get; init; }

    public double StartValue { get; init; }
    public double EndValue { get; init; }



    public HillDescendingSetting(int hardStop, double averageElementsChanged, double acceptanceDecay, double startValue, double endValue) : base(hardStop)
    {
        AverageElementsChanged = averageElementsChanged;
        AcceptanceDecay = acceptanceDecay;
        StartValue = startValue;
        EndValue = endValue;
    }
}