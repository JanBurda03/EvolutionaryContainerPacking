public static class GeneratingProgram
{
    public static void Main()
    {
        Generate("C:\\Users\\Jan Burda\\Desktop\\BachelorThesis\\Code\\TestInputs\\PackingInput.JSON");
    }
    
    public static void Generate(string fileName)
    {
        var factory = new RandomBoxPropertyFactory(5, 20, 0.01, 0.05);
        var boxes = factory.CreateMultiple(100);

        var container = new ContainerProperties(new Sizes(50, 40, 30), int.MaxValue);

        PackingInput packingInput = new PackingInput(container, boxes);
        PackingInputSaver.SaveToFile(packingInput, fileName);
    }
}