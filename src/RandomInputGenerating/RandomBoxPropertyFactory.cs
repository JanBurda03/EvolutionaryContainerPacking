
public class RandomBoxPropertyFactory
{
    private int _currentID;
    private Random _random;
    private readonly int MinSize;
    private readonly int MaxSize;
    private readonly double MinDensity;
    private readonly double MaxDensity;


    public RandomBoxPropertyFactory(int minSize, int maxSize, double minDensity, double maxDensity)
    {
        MinDensity = minDensity;
        MaxDensity = maxDensity;
        MinSize = minSize;
        MaxSize = maxSize;
        _random = new Random();
        _currentID = 0;
    }
    private double NextDensity()
    {
        return _random.NextDouble() * (MaxDensity - MinDensity) + MinDensity;
    }
    private int NextSize()
    {
        return _random.Next(MinSize, MaxSize);
    }
    public BoxProperties Create()
    {
        Sizes sizes = new Sizes(NextSize(), NextSize(), NextSize());
        return new BoxProperties(_currentID++, sizes, (int)(sizes.GetVolume() * NextDensity()));
    }

    public IReadOnlyList<BoxProperties> CreateMultiple(int number)
    {
        BoxProperties[] boxProperties = new BoxProperties[number];
        for (int i = 0; i < number; i++) 
        {
            boxProperties[i] = Create();
        }
        return boxProperties;
    }
}