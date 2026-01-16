public readonly record struct IterationData<T>
{
    public IterationData(int iterationNumber, double bestScore, double averageScore)
    {
        IterationNumber = iterationNumber;
        BestScore = bestScore;
        AverageScore = averageScore;
    }

    public int IterationNumber { get; init; }
    public readonly double BestScore { get; init; }
    public readonly double AverageScore { get; init; }
}