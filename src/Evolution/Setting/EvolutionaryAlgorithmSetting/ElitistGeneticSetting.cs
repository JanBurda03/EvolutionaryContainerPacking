public record ElitistGeneticSetting : EvolutionaryAlgorithmSetting
{
    public int NumberOfElitists {  get; init; }
    public double AverageElementsMutated { get; init; }
    public double AverageElementsFromNonElite { get; init; }

    public int NumberOfNew { get; init; }

    public ElitistGeneticSetting(int hardStop, int numberOfElitists, double averageElementsFromNonElite, double averageElementsMutated, int numberOfNew) : base(hardStop)
    {
        if (numberOfElitists <= 0)
        {
            throw new ArgumentException("Tournament Size must always be greater than 0");
        }

        NumberOfElitists = numberOfElitists;
        AverageElementsMutated = averageElementsMutated;
        AverageElementsFromNonElite = averageElementsFromNonElite;
        NumberOfNew = numberOfNew;
    }
}