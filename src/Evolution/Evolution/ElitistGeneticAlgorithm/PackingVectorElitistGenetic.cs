public class PackingVectorElitistGenetic : ElitistGenetic<PackingRules>
{    
    public PackingVectorElitistGenetic(IReadOnlyList<PackingRules> initialPopulation, IFitnessEvaluator<PackingRules> fitnessEvaluator, ElitistGeneticSetting setting, IIterationStatistics<PackingRules>? data = null) 
        : base(initialPopulation, fitnessEvaluator, new PackingRulesUniformCrossover(setting.AverageElementsFromNonElite), new PackingRulesMutation(setting.AverageElementsMutated), new PackingRulesPopulationFactory(initialPopulation.First().Count),setting, data)
    {}
}